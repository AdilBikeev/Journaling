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
    }
}
