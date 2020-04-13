using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace JournalingGUI.Models
{
    [Serializable]
    public class StateApplication
    {
        /// <summary>
        /// Имя файла состояний
        /// </summary>
        public const string fileName = "state_app.xml";

        [XmlElement(ElementName ="File")]
        public FileModel file { get; set; }

        [XmlElement(ElementName = "IsNewFile")]
        public bool IsNewFile { get; set; }

        [XmlElement(ElementName = "GeneralLog")]
        public string GeneralLog { get; set; }
    }
}
