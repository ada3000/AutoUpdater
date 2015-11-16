using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using iTeco.Lib.Base;
using iTeco.Lib.Srv;

namespace AK.Service.Test
{
	public partial class Form1 : Form
	{
		System.Diagnostics.EventLog _logStream = new System.Diagnostics.EventLog("Application");
		/// <summary>
		/// Автозапуск сервиса
		/// </summary>
		public bool AutoStart = false;

		private ServiceMain _service = null;

		/// <summary>
		/// Путь к текущей конфигурации
		/// </summary>
		private string _activeConfigPath = null;

		public Form1()
		{
			InitializeComponent();

			_logStream.Source = "Srv.Common Test App";
			_logStream.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(LogStream_EntryWritten);
		}

		void LogStream_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
		{
			ToLog(e.Entry.Message);
		}

		private void cbConfigFileName_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_activeConfigPath != null)
				DeleteConfig();


			if (cbConfigFileName.SelectedIndex == -1)
			{
				txtXmlConfig.Text = "";
				return;
			}
			else
			{
				LoadConfig();
			}
		}
		/// <summary>
		/// Загрузка конф файла
		/// </summary>
		private void LoadConfig()
		{
			try
			{
				string xml = File.ReadAllText(cbConfigFileName.Text);
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);
				XmlNodeList items = doc.SelectNodes("//_fileReference");
				if(items!=null && items.Count>0)
					foreach (XmlElement elt in items)
					{
						string subXml = File.ReadAllText(elt.GetAttribute("file"));
						XmlElement container = doc.CreateElement("c");
						container.InnerXml=subXml;

						XmlNodeList subItems = container.SelectNodes("."+elt.GetAttribute("nodes_path"));
						if (subItems != null && subItems.Count > 0)
							foreach (XmlNode sub in subItems)
								elt.ParentNode.InsertAfter(sub,elt);

						elt.ParentNode.RemoveChild(elt);
					}
				StringBuilder sbXml=new StringBuilder();
				XmlWriterSettings settings= new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars="\t";
				settings.NewLineChars="\r\n";
				settings.NewLineHandling = NewLineHandling.Replace;

				using (XmlWriter writer = XmlWriter.Create(sbXml, settings))
					doc.Save(writer);
				txtXmlConfig.Text = sbXml.ToString();
				//txtXmlConfig.ConfigurationManager.Language = "xml";
				//txtXmlConfig.Margins[0].Width = 20;
			}
			catch (Exception ex)
			{
				txtXmlConfig.Text = ex.ToString();
			}
		}
		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void tsbStart_Click(object sender, EventArgs e)
		{
			StartService();
		}
		/// <summary>
		/// Запуск сервиса
		/// </summary>
		private void StartService()
		{
			try
			{
				if (_service != null)
				{
					_service.Stop();
					_service = null;
				}
				DeleteConfig();
				SaveConfig();

				_service = new ServiceMain(new ServiceConfigFileEx(_activeConfigPath));
				_service.Cfg.Read();
				//_service = new ServiceMain(new iTeco.Lib.Srv.ServiceConfigFile("configCommon"));
				_service.Event.Log = _logStream;
				_service.Event.Error = Service_Error;
				_service.Start();
				ToLog("Сервис запущен");
			}
			catch (Exception e)
			{
				ToLog("Ошибка запуска сервиса: " + e.ToString());
			}
		}
		private void Service_Error(ServiceMain obj, EventArgs e)
		{
			if (obj.Error == null) return;
			ToLog(obj.Error.ToString());
		}

		/// <summary>
		/// Логирование
		/// </summary>
		/// <param name="msg"></param>
		private void ToLog(string msg)
		{
			if (InvokeRequired)
			{
				Action<string> d = new Action<string>(ToLog);
				Invoke(d, msg);
				return;
			}
			lbLog.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " " + msg);
			lbLog.SelectedIndex = lbLog.Items.Count - 1;
		}
		private void SaveConfig()
		{
			if (_activeConfigPath == null)
				_activeConfigPath = Path.GetTempFileName();

			File.WriteAllText(_activeConfigPath, txtXmlConfig.Text);
		}
		private void DeleteConfig()
		{
			if (_activeConfigPath != null)
			{
				File.Delete(_activeConfigPath);
				_activeConfigPath = null;
			}
		}
		private void tsbUpdateConfig_Click(object sender, EventArgs e)
		{
			UpdateConfig();
		}
		/// <summary>
		/// Обновить параметры конфигурации
		/// </summary>
		private void UpdateConfig()
		{
			if (_service == null || _activeConfigPath == null) return;

			SaveConfig();
			_service.Cfg.Read();

			ToLog("Параметры обновлены");
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			if (cbConfigFileName.Items.Count > 0)
				cbConfigFileName.SelectedIndex = 0;

			if (AutoStart)
			{
				tsbStart.PerformClick();
				WindowState = FormWindowState.Minimized;					
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F5:
					tsbStart.PerformClick();
					break;
				case Keys.S:
					if (e.Control)
						tsbUpdateConfig.PerformClick();
					break;
			}
		}

		private void cbLoadConfig_Click(object sender, EventArgs e)
		{
			if (_activeConfigPath != null)
				DeleteConfig();


			LoadConfig();
		}
	}
}
