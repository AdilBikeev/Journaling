using System;
using System.Collections.Generic;
using System.Text;

namespace JournalingGUI.Models
{
    /// <summary>
    /// Описывает файлы
    /// </summary>
    public class FileModel
    {
        public const string DefaultFileName = "+"; 

        /// <summary>
        /// Название файла с расширением
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public string body { get; set; }

        public override string ToString()
        {
            return fileName; 
        }

        /// <summary>
        /// Создает дефолтный объект для обозначения формирования нового файла.
        /// </summary>
        public FileModel()
        {
            this.body = string.Empty;
            this.fileName = FileModel.DefaultFileName;
        }
    }
}
