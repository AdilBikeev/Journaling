using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JournalingGUI.Hellpers;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace JournalingGUI.Controller
{
    public class FileSystemController
    {
        /// <summary>
        /// Путь к файловой системе
        /// </summary>
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "file_system");

        /// <summary>
        /// Время обновления файловой системы в мс.
        /// </summary>
        private static readonly int timeUpdate = 1000;

        /// <summary>
        /// Список файлов
        /// </summary>
        public ObservableCollection<FileModel> filesList = new ObservableCollection<FileModel>();

        public FileSystemController()
        {
        }

        public void UpdateFileSystemAsync()
        {
            TaskHellpers task = new TaskHellpers();
            task.StartTimer(this.Load, FileSystemController.timeUpdate);
        }

        /// <summary>
        /// Загружает все данные файловой системы
        /// </summary>
        public void Load()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(this.path);
                JournalFileSystemController journal = new JournalFileSystemController();
                ObservableCollection<FileModel> currentFiles = new ObservableCollection<FileModel>();

                foreach (var file in directory.GetFiles())
                {
                    var fileModel = new FileModel()
                    {
                        body = FileSystemHellpers.GetBodyFile(file.FullName),
                        fileName = file.Name
                    };

                    currentFiles.Add(fileModel);
                    journal.Add(fileModel, this.filesList);
                }
                journal.CheckDeleteFiles(this.filesList, currentFiles);
                FileSystemHellpers.UpdateFileList(this.filesList, journal);
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error("Ошибка", exc.Message);   
            }
        }
    }
}
