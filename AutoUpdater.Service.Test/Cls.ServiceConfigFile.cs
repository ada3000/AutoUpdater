//-----------------------------------------------------------------------
// Cls.ServiceConfigFile.cs - классы реализации настроек сервиса с
//	использованием xml-файла, путь до которого указан в app-config
//
// Created by Ozzy 23.12.2011
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Text;
using System.Xml;

using iTeco.Lib.Base;
using iTeco.Lib.Srv;


namespace iTeco.Lib.Srv
{
	/// <summary>
	/// Класс настроек сервиса использующий в качестве источника xml-файл
	/// Путь до файла получается из app-config, из указанноой секции
	/// </summary>
	public sealed class ServiceConfigFileEx : ServiceMain.Config
	{
		private Settings _settings;

		/// <summary>
		/// Реальный конструктор класса
		/// </summary>
		/// <param name="settings">Объект реализующий реальную работу с файлом</param>
		public ServiceConfigFileEx(string fileName)
			: base(null)
		{
			_settings = new Settings(fileName);
		}
		public override void Read()
		{
			Xml = _settings.Read();
		}
		public override void Write()
		{
			_settings.Write( Xml );
		}

		/// <summary>
		/// Класс объекта непосредственно выполняющего все телодвижения с файлом
		/// </summary>
		private sealed class Settings
		{
			private string _fileName = null;
			/// <summary>
			/// Название секции app-config, в которой задан путь до файла конфигурации
			/// </summary>
			private string _section = null;

			/// <summary>
			/// Путь до файла конфигурации
			/// </summary>
			public string FilePath
			{
				get
				{
					if (_fileName.IsNoValue())
						throw new NotImplementedException(
							string.Format( "не установлен путь к файлу конфигурации в секции {0} настроек приложения",_section ));
					return Path.IsPathRooted(_fileName) ? _fileName : AppDomain.CurrentDomain.BaseDirectory + _fileName;
				}
			}

			/// <summary>
			/// Конструктор
			/// </summary>
			/// <param name="sectionName">Название секции app-config</param>
			public Settings( string fileName )
			{
				if (fileName.IsNoValue())
					throw new ArgumentException("не задана секция настроек приложения, из которой должен быть получен путь к файлу конфигурации","sectionName");
				_fileName = fileName;
			}

			public XmlDocument Read()
			{
				XmlDocument xml;
				using (StreamReader file = new StreamReader(FilePath))
				{
					xml = file.ToXmlDocument();
				}
				return xml;
			}
			public void Write( XmlDocument xml )
			{
				if(xml != null)
				{
					var settings = new XmlWriterSettings();
					using (StreamWriter file = new StreamWriter(FilePath, false, settings.Encoding))
					{
						xml.Save( XmlWriter.Create( file,settings ) );
					}
				}
			}
		}
	}
}
