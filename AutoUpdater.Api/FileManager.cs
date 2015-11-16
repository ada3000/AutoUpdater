using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdater.Api
{
	public class FileManager
	{
		private const string PackageDescFileName = "package.xml";
		private const string VersionFileName = "version.txt";
		private const string DownloadVersionFileName = "downloadVersion.txt";
		private const string DistFileName = "dist.zip";
		private const string UpdateProcessName = "Updater.exe";
		
		private string _workFolder = null;
		private string _workFolderPath = null;

		public FileManager(string workFolder)
		{
			_workFolder = workFolder;
		}
		public void SavePackageDesc(string packageXml)
		{
			var filePath = Path.Combine(GetWorkFolder(), PackageDescFileName);
			File.WriteAllText(filePath, packageXml);
		}

		public string LoadPackageDesc()
		{
			var filePath = Path.Combine(GetWorkFolder(), PackageDescFileName);
			return File.ReadAllText(filePath);
		}

		public Stream GetDistStream(bool createNew)
		{
			var filePath = Path.Combine(GetWorkFolder(), DistFileName);
			FileStream fs = new FileStream(filePath, createNew ? FileMode.Create : FileMode.OpenOrCreate, FileAccess.ReadWrite);
			return fs;
		}

		public string GetDistPath()
		{
			var filePath = Path.Combine(GetWorkFolder(), DistFileName);
			return filePath;
		}
		/// <summary>
		/// Установленная версия
		/// </summary>
		public string InstalledVersion
		{
			get
			{
				var filePath = Path.Combine(GetWorkFolder(), VersionFileName);

				if (File.Exists(filePath))
					return File.ReadAllText(filePath);

				return null;
			}
			set
			{
				var filePath = Path.Combine(GetWorkFolder(), VersionFileName);
				File.WriteAllText(filePath, value);
			}
		}

		/// <summary>
		/// Скаченная версия
		/// </summary>
		public string DownloadedVersion
		{
			get
			{
				var filePath = Path.Combine(GetWorkFolder(), DownloadVersionFileName);

				if (File.Exists(filePath))
					return File.ReadAllText(filePath);

				return null;
			}
			set
			{
				var filePath = Path.Combine(GetWorkFolder(), DownloadVersionFileName);
				File.WriteAllText(filePath, value);
			}
		}

		/// <summary>
		/// Скаченная версия
		/// </summary>
		public string UpdaterAppPath
		{
			get
			{
				var filePath = Path.Combine(GetCurrentFolder(), UpdateProcessName);

				return filePath;
			}
		}
		private string GetWorkFolder()
		{
			if (_workFolderPath != null) return _workFolderPath;

			_workFolderPath = System.Reflection.Assembly.GetEntryAssembly()
				.CodeBase
				.Substring(8)
				.Replace("/", "\\");

			_workFolderPath = _workFolderPath.Substring(0, _workFolderPath.LastIndexOf("\\"));
			_workFolderPath = Path.Combine(_workFolderPath, _workFolder);
			if (!Directory.Exists(_workFolderPath)) Directory.CreateDirectory(_workFolderPath);

			return _workFolderPath;
		}

		private string GetCurrentFolder()
		{
			string folder = System.Reflection.Assembly.GetEntryAssembly()
				.CodeBase
				.Substring(8)
				.Replace("/", "\\");

			folder = _workFolderPath.Substring(0, _workFolderPath.LastIndexOf("\\"));

			return folder;
		}
	}
}
