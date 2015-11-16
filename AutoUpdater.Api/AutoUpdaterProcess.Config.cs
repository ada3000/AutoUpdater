using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoUpdater.Api
{
    [XmlRoot("process")]
	public class AutoUpdaterProcessConfig
    {
        /// <summary>
        /// Адрес сервера обновлений
        /// </summary>
        [XmlElement("sourceUrl")]
        public string SourceUrl;
        /// <summary>
        /// Имя сервера
        /// </summary>
        [XmlElement("instanceName")]
        public string InstanceName;
		/// <summary>
		/// Интервал проверки новой версии в минутах
		/// </summary>
		[XmlElement("checkDelayMin")]
		public int CheckDelayMin;
		/// <summary>
		/// Рабочая папка для загрузки обновлений
		/// </summary>
		[XmlElement("workFolder")]
		public string WorkFolder;
    }
}
