using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;

using iTeco.Lib.Base;

using log4net;

using AutoUpdater.Api;
using Microsoft.Win32;
using System.Diagnostics;

namespace Updater
{
	class Program
	{
		private const int MaxUpdateTries = 5;

		static readonly ILog Logger = LogManager.GetLogger("AutoUpdaterProcess");
		static FileManager _mgr = null;

        static PackageConfig _package = null;

        static void Main(string[] args)
		{
			Logger.Debug("Updater proc ...");

			try
			{
				if (args == null && args.Length != 1)
					throw new ArgumentException("Входная папка не задана");

				_mgr = new FileManager(args[0]);
                _package = _mgr.LoadPackageDesc().ToXmlReader().Deserialize<PackageConfig>();

                if (_mgr.InstalledVersion == _mgr.DownloadedVersion)
				{
					Logger.Debug("Service already updated!");
					return;
				}

                StartAndWaitAction("OnBeforeUpdateAction", _package.OnBeforeUpdateAction);

                TryUpdate();

                StartAndWaitAction("OnSuccessUpdateAction", _package.OnSuccessUpdateAction);
            }
			catch (Exception ex)
			{
				Logger.Error("Updater proc: " + ex);
                StartAndWaitAction("OnErrorUpdateAction", _package.OnErrorUpdateAction);
            }

			Logger.Debug("Updater proc: end");
		}

        static void StartAndWaitAction(string name, string command)
        {
            Logger.Debug("Updater proc: StartAction name="+name+" cmd="+command);
            if (string.IsNullOrEmpty(command))
            {
                Logger.Debug("Updater proc: StartAction empty action");
                return;
            }

            try
            {
                var p=Process.Start(command);
                p.WaitForExit();
                if (p.ExitCode != 0)
                    throw new Exception("Invalid ExitCode=" + p.ExitCode);

                Logger.Debug("Updater proc: StartAction sucess");
            }
            catch(Exception ex)
            {
                Logger.Error("Updater proc: StartAction "+ex);
            }
        }

		private static void TryUpdate()
		{
			Exception lastError = null;

			for (int i = 0; i < MaxUpdateTries; i++)
				try
				{
					DoUpdate();
					break;
				}
				catch (Exception innerEx)
				{
					lastError = innerEx;
				}

			if (lastError != null)
				throw lastError;
		}

		private static void DoUpdate()
		{
			string sourceFolder = null;

			try
			{
				sourceFolder = UnpackDist();
				string serviceFolder = GetServiceFolder(_package.ServiceName);

				StopService(_package.ServiceName);

				UpdateFiles(sourceFolder, serviceFolder, _package.SkipFilesIfExists);
				UpdateVersion(_package.Version);

				StartService(_package.ServiceName);
			}
			finally
			{
				if (!string.IsNullOrEmpty(sourceFolder))
					ClearFolder(sourceFolder);
			}
		}

		private static void UpdateVersion(string version)
		{
			_mgr.InstalledVersion = version;
		}
		/// <summary>
		/// Очистка временной папки
		/// </summary>
		/// <param name="sourceFolder"></param>
		private static void ClearFolder(string sourceFolder)
		{
			Logger.Debug("ClearFolder ...");

			try
			{
				var files = Directory.GetFiles(sourceFolder);
				foreach (string file in files)
					File.Delete(Path.Combine(sourceFolder, file));

				Directory.Delete(sourceFolder);
			}
			catch (Exception ex)
			{
				Logger.Error("ClearFolder: " + ex);
			}

			Logger.Debug("ClearFolder end");
		}

		private static void StartService(string serviceName)
		{
			Logger.Debug("StartService ...");

			ServiceController sc = new ServiceController(serviceName);
			if (sc.Status != ServiceControllerStatus.Running)
			{
				sc.Start();

				int cnt = 0;
				while (cnt < 30 && sc.Status != ServiceControllerStatus.Running)
				{
					cnt++;
					Thread.Sleep(1000);
					sc.Refresh();
				}
			}

			if (sc.Status != ServiceControllerStatus.Running)
				throw new Exception("can't start service! serviceName=" + serviceName);

			Logger.Debug("StartService end");

		}

		private static void UpdateFiles(string sourceFolder, string destFolder, string[] skipFiles)
		{
			Logger.Debug("UpdateFiles ...");

			skipFiles = skipFiles ?? new string[] { };

			string[] files = Directory.GetFiles(sourceFolder);

			foreach (var file in files)
			{
				var sourceFileName = file.Substring(file.LastIndexOf("\\") + 1);

				if (skipFiles.Contains(sourceFileName)) continue;

				var destFile = Path.Combine(destFolder, sourceFileName);

				File.Copy(file, destFile, true);
			}

			Logger.Debug("UpdateFiles end");
		}

		private static void StopService(string serviceName)
		{
			Logger.Debug("StopService ...");

			ServiceController sc = new ServiceController(serviceName);
			if (sc.CanStop)
			{
				sc.Stop();

				int cnt = 0;
				while (cnt < 30 && sc.Status != ServiceControllerStatus.Stopped)
				{
					cnt++;
					Thread.Sleep(1000);
					sc.Refresh();
				}

				if (sc.Status != ServiceControllerStatus.Stopped)
					throw new Exception("can't stop service! serviceName=" + serviceName);
			}

			Thread.Sleep(20000);

			Logger.Debug("StopService end");
		}

		//private static void CopySkipFiles(string[] files, string sourceFolder, string destFolder)
		//{
		//	if (files == null) return;

		//	Logger.Debug("CopySkipFiles ...");

		//	foreach (var file in files)
		//	{
		//		var sourceFile = Path.Combine(sourceFolder, file);
		//		var destFile = Path.Combine(destFolder, file);

		//		File.Copy(sourceFile, destFile, true);
		//	}

		//	Logger.Debug("CopySkipFiles end");
		//}

		private static string GetServiceFolder(string serviceName)
		{
			//HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\ + ServiceName -> ImagePath
			var key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\" + serviceName, false);
			string path = key.GetValue("ImagePath").ToString().Substring(1);
			path = path.Substring(0, path.LastIndexOf("\\"));

			return path;
		}

		private static string UnpackDist()
		{
			Logger.Debug("UnpackDist ...");
			string tempFolder = Path.Combine(Path.GetTempPath(), "AutoUpdater." + Guid.NewGuid());
			Directory.CreateDirectory(tempFolder);

			string distZip = _mgr.GetDistPath();

			ZipFile.ExtractToDirectory(distZip, tempFolder);

			Logger.Debug("UnpackDist end, folder=" + tempFolder);

			return tempFolder;
		}
	}
}
