using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JournalingGUI.Models
{
    public abstract class BaseFileSystem
    {
        /// <summary>
        /// Загружает все данные файловой системы
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Сохраняет файл в файловой системе
        /// </summary>
        public abstract void Save(FileModel file);

        /// <summary>
        /// Проверяет есть ли файл с таким же именем.
        /// </summary>
        /// <param name="fileFullName">Полное Имя файла.</param>
        protected virtual bool IsFileExist(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
