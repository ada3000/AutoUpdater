using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoUpdater.Api
{
    [XmlRoot("process")]
    public class ClientProcessConfig
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
    }
}
