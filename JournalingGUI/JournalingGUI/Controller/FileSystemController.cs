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
                        fileName = file.Name.Substring(0,file.Name.LastIndexOf('.')),
                        extension = file.Extension
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
    
        /// <summary>
        /// Сохраняет файл в файловой системе
        /// </summary>
        public void Save(FileModel file, bool isNewFile)
        {
            if (IsFileExist(Path.Combine(this.path, $"{file.fileName}.{file.extension}")))
            {
                var result = MessageBoxHellpers.Questions("Файл с данным именем уже существует, вы уверены, что хотите перезаписать данные в нём ?");

                switch (result)
                {
                    case System.Windows.MessageBoxResult.Cancel:
                        return;
                    case System.Windows.MessageBoxResult.Yes:
                        break;
                    case System.Windows.MessageBoxResult.No:
                        FileSystemHellpers.SetOriginalFileName(this.path, file);
                        break;
                    default:
                        break;
                }
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(this.path, $"{file.fileName}.{file.extension}")))
                {
                    sw.Write(file.body);
                }
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error($"Исключение {nameof(FileSystemController)}.Save", exc.Message);
            }
        }

        /// <summary>
        /// Удаляет файл из файловой системы.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public void Delete(string fileName)
        {
            var path = Path.Combine(this.path, fileName);

            if(IsFileExist(path))
            {
                File.Delete(path);
            } else
            {
                MessageBoxHellpers.Error("Ошибка", $"Файл с именем: {fileName} не существует в дириктории {path}");
            }
        }

        /// <summary>
        /// Проверяет есть ли файл с таким же именем.
        /// </summary>
        /// <param name="fileFullName">Полное Имя файла.</param>
        private bool IsFileExist(string fileFullName)
        {
            if(File.Exists(fileFullName))
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
