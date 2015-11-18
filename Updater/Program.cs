using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

using iTeco.Lib.Base;

using AutoUpdater.Api;
using Microsoft.Win32;
using System.ServiceProcess;
using System.Threading;

namespace Updater
{
	class Program
	{
		static readonly ILog Logger = LogManager.GetLogger("AutoUpdaterProcess");
		static FileManager _mgr = null;

		static void Main(string[] args)
		{
			Logger.Debug("Updater proc ...");

			string sourceFolder = null;

			try
			{
				if (args == null && args.Length != 1)
					throw new ArgumentException("Входная папка не задана");

				_mgr = new FileManager(args[0]);

				if(_mgr.InstalledVersion==_mgr.DownloadedVersion)
				{
					Logger.Debug("Service already updated!");
					return;
				}

				PackageConfig package = _mgr.LoadPackageDesc().ToXmlReader().Deserialize<PackageConfig>();

				sourceFolder = UnpackDist();
				string serviceFolder = GetServiceFolder(package.ServiceName);

				CopySkipFiles(package.SkipFilesIfExists, serviceFolder, sourceFolder);
				
				StopService(package.ServiceName);
				
				UpdateFiles(sourceFolder, serviceFolder);
				UpdateVersion(package.Version);

				StartService(package.ServiceName);
			}
			catch (Exception ex)
			{
				Logger.Error("Updater proc: " + ex);
			}
			finally
			{
				if (!string.IsNullOrEmpty(sourceFolder))
					ClearFolder(sourceFolder);
			}

			Logger.Debug("Updater proc: end");
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
			catch(Exception ex)
			{
				Logger.Error("ClearFolder: "+ex);
			}

			Logger.Debug("ClearFolder end");
		}

		private static void StartService(string serviceName)
		{
			Logger.Debug("StartService ...");

			ServiceController sc = new ServiceController(serviceName);
			if (sc.Status!= ServiceControllerStatus.Running)
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

		private static void UpdateFiles(string sourceFolder, string destFolder)
		{
			Logger.Debug("UpdateFiles ...");

			string[] files = Directory.GetFiles(sourceFolder);

			foreach (var file in files)
			{
				var sourceFileName = file.Substring(file.LastIndexOf("\\")+1);
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

		private static void CopySkipFiles(string[] files, string sourceFolder, string destFolder)
		{
			if (files == null) return;

			Logger.Debug("CopySkipFiles ...");

			foreach (var file in files)
			{
				var sourceFile = Path.Combine(sourceFolder, file);
				var destFile = Path.Combine(destFolder, file);

				File.Copy(sourceFile, destFile, true);
			}

			Logger.Debug("CopySkipFiles end");
		}

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
