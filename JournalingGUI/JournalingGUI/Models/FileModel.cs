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
        /// Расширение для файлов по умолчанию.
        /// </summary>
        public const string DefaultExtension = ".txt";

        /// <summary>
        /// Название файла с расширением
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string extension { get; set; }

        public override string ToString()
        {
            if (this.fileName == FileModel.DefaultFileName)
                return FileModel.DefaultFileName;
            else
                return $"{this.fileName}{this.extension}"; 
        }

        /// <summary>
        /// Создает дефолтный объект для обозначения формирования нового файла.
        /// </summary>
        public FileModel()
        {
            this.body = string.Empty;
            this.fileName = FileModel.DefaultFileName;
            this.extension = FileModel.DefaultExtension;
        }
    }
}
