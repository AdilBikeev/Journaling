using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JournalingGUI.Hellpers;

namespace JournalingGUI.Controller
{
    public class FileSystemController
    {
        /// <summary>
        /// Путь к файловой системе
        /// </summary>
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "file_system");

        /// <summary>
        /// Список файлов
        /// </summary>
        public List<FileModel> filesList = new List<FileModel>();

        public FileSystemController()
        {
            this.Load();
        }

        /// <summary>
        /// Загружает все данные файловой системы
        /// </summary>
        public void Load()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(this.path);

                foreach (var file in directory.GetFiles())
                {
                    var fileModel = new FileModel()
                    {
                        body = FileSystemHellpers.GetBodyFile(file.FullName),
                        fileName = file.Name
                    };

                    filesList.Add(fileModel);
                }
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error("Ошибка", exc.Message);   
            }
        }
    }
}
