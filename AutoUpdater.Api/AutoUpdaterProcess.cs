using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using iTeco.Lib.Base;
using iTeco.Lib.Srv;
using log4net;
using System.Diagnostics;

namespace AutoUpdater.Api
{
	public class AutoUpdaterProcess : ServiceProcessBase
    {
		private const string PackageDescFileName = "package.xml";
		
		private DateTime _lastCheck = DateTime.MinValue;

		private PackageConfig _lastConfig = null;

		private FileManager _fileManager = null;
		private readonly ILog Logger = LogManager.GetLogger("AutoUpdaterProcess");
		private AutoUpdaterProcessConfig ProcessConfig { get; set; }

		private int SleepMSec = 60000;
		private int SleepErrorMSec = 600000;

		private void InitConfig()
		{
			ProcessConfig = Cfg.Xml.ToXmlReader().Deserialize<AutoUpdaterProcessConfig>();

			_fileManager = new FileManager(ProcessConfig.WorkFolder);
		}

        protected override void DoWork(object p)
        {
            Debug("Start process");
			Event.CfgChanged.Set();

            while(Event.Stopping.IsReset())
            {
                try
                {
					if (Event.CfgChanged.IsSet())
					{
						Debug("CfgChanged ...");
						InitConfig();
						Event.CfgChanged.Reset();
						Debug("CfgChanged end");
					}

					if (CanCheck())
					{
						if (HasVersion())
						{
							DownloadDist();
							StartUpdate();
						}

						_lastCheck = DateTime.UtcNow;
					}

					Event.Wait(SleepMSec);
                }
                catch(Exception ex)
                {
					Logger.Error(Cfg.Name+": " + ex);
					Event.Wait(SleepErrorMSec);
                }
            }

			Debug("Stop process");
        }

		private void StartUpdate()
		{
			Debug("StartUpdate ...");
			Process p = Process.Start(_fileManager.UpdaterAppPath, ProcessConfig.WorkFolder);

			Debug("StartUpdate end");
		}

		private bool CanCheck()
		{
			return _lastCheck.AddMinutes(ProcessConfig.CheckDelayMin) < DateTime.UtcNow;
		}
		private bool HasVersion()
		{
			Debug("CheckNewVersion ...");

			var url = ProcessConfig.SourceUrl;
			if (url[url.Length - 1] != '/') url += "/";

			string packageXml = HttpDownloader.Get(url + PackageDescFileName);
			_lastConfig = packageXml.ToXmlReader().Deserialize<PackageConfig>();

			if (_fileManager.InstalledVersion != _lastConfig.Version)
			{
				Debug("CheckNewVersion end, found new version = " + _lastConfig.Version);
				_fileManager.SavePackageDesc(packageXml);

				return true;
			}
			else
				Debug("CheckNewVersion end, not found new version");

			return false;
		}

		private void DownloadDist()
		{
			var url = ProcessConfig.SourceUrl;
			if (url[url.Length - 1] != '/') url += "/";

			Debug("DownloadDist ... url=" + url + _lastConfig.DistUrl);

			if (_lastConfig.Version == _fileManager.DownloadedVersion)
			{
				Debug("DownloadDist this version already downloaded! version=" + _lastConfig.Version);
			}
			else
			{
				using (Stream fs = _fileManager.GetDistStream(true))
					HttpDownloader.Get(url + _lastConfig.DistUrl, fs);

				_fileManager.DownloadedVersion = _lastConfig.Version;
			}

			Debug("DownloadDist end");
		}
		
        protected override void DoError(Exception err, object p)
        {
            base.DoError(err, p);
			Error(" unexpected exception! " + err);
        }

		protected void Debug(string msg)
		{
			Logger.Debug(Cfg.Name+": "+ msg);
		}
		protected void DebugFormat(string msg, params object[] arguments)
		{
			Logger.DebugFormat(Cfg.Name + ": " + msg, arguments);
		}
		protected void Error(string msg)
		{
			Logger.Error(Cfg.Name + ": " + msg);
		}
    }
}
