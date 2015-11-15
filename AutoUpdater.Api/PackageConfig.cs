using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoUpdater.Api
{
    [XmlRoot("packageConfig")]
    public class PackageConfig
    {
        /// <summary>
        /// Название приложения
        /// </summary>
        [XmlElement("name")]
        public string Name;
        /// <summary>
        /// Версия приложения
        /// </summary>
        [XmlElement("version")]
        public string Version;
        /// <summary>
        /// Версия приложения
        /// </summary>
        [XmlElement("serviceName")]
        public string ServiceName;
        /// <summary>
        /// Версия приложения
        /// </summary>
        [XmlElement("distUrl")]
        public string DistUrl;

        /// <summary>
        /// Выполнение приложения перед обновлением
        /// </summary>
        [XmlElement("onBeforeUpdateAction")]
        public string OnBeforeUpdateAction;
        /// <summary>
        /// Выполнение приложения после успешного обновления
        /// </summary>
        [XmlElement("onSuccessUpdateAction")]
        public string OnSuccessUpdateAction;
        /// <summary>
        /// Выполнение приложения после неуспешного обновления
        /// </summary>
        [XmlElement("onErrorUpdateAction")]
        public string OnErrorUpdateAction;

        /// <summary>
        /// Пропуск существующих файлов при обновлении
        /// </summary>
        [XmlArray("skipFilesIfExists"), XmlArrayItem("f")]
        public string[] SkipFilesIfExists;
    }
}
