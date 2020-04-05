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
using System.Threading;

namespace JournalingGUI.Controller
{
    public class FileSystemController : BaseFileSystem
    {
        /// <summary>
        /// Путь к файловой системе
        /// </summary>
        protected override string path { get; set; }

        /// <summary>
        /// Время обновления файловой системы в мс.
        /// </summary>
        private static readonly int timeUpdate = 3000;

        /// <summary>
        /// Время сна перед выполнением операции с файловой системой
        /// </summary>
        private static readonly int timeOperation = 5000;

        /// <summary>
        /// Список файлов
        /// </summary>
        public ObservableCollection<FileModel> filesList = new ObservableCollection<FileModel>();

        /// <summary>
        /// Контроллер для backup фалйовой системы.
        /// </summary>
        private BackupController backup = new BackupController();

        public FileSystemController()
        {
            this.path = Path.Combine(Directory.GetCurrentDirectory(), "file_system");
        }

        public void UpdateFileSystemAsync()
        {
            TaskHellpers task = new TaskHellpers();
            task.StartTimer(this.Load, FileSystemController.timeUpdate);
        }

        ///<inheritdoc/>
        public override void Load()
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
    
        ///<inheritdoc/>
        public override bool Save(FileModel file)
        {
            Thread.Sleep(FileSystemController.timeOperation);
            if (IsFileExist(Path.Combine(this.path, $"{file.fileName}{file.extension}")))
            {
                var result = MessageBoxHellpers.Questions("Файл с данным именем уже существует, вы уверены, что хотите перезаписать данные в нём ?");

                switch (result)
                {
                    case System.Windows.MessageBoxResult.Cancel:
                        return false;
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
                using (StreamWriter sw = new StreamWriter(Path.Combine(this.path, $"{file.fileName}{file.extension}")))
                {
                    sw.Write(file.body);
                }

                return true;
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error($"Исключение {nameof(FileSystemController)}.Save", exc.Message);
            }

            return false;
        }

        /// <summary>
        /// Удаляет файл из файловой системы.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public void Delete(FileModel file)
        {
            JournalFileSystemController.AddInfo($"Начало удаление файла {file.ToString()}");
            Thread.Sleep(FileSystemController.timeOperation);
            var path = Path.Combine(this.path, file.ToString());

            if(IsFileExist(path))
            {
                File.Delete(path);
                JournalFileSystemController.AddInfo($"Конец операции удаление файла {file.ToString()}");
            } else
            {
                MessageBoxHellpers.Error("Ошибка", $"Файл с именем: {file.ToString()} не существует в дириктории {path}");
                JournalFileSystemController.AddInfo($"Ошибка при удалении файла {file.ToString()}");
            }
        }

        /// <summary>
        /// Сохраняет текущую сессию в backup
        /// </summary>
        public void SaveSession()
        {
            this.backup.ClearFileSystem();
            this.backup.SaveAll(this.filesList);
            JournalFileSystemController.AddInfo("Сессия сохранена");
        }

        public void SaveState(StateApplication state) => this.backup.SaveState(state);

        /// <summary>
        /// Выполняет запрос на восстановление файловой системы
        /// </summary>
        /// <returns></returns>
        public bool RestoreFileSystem(out StateApplication state)
        {
            var result = MessageBoxHellpers.Questions("Восстанвоить файловую систему ?");
            switch (result)
            {
                case System.Windows.MessageBoxResult.None:
                    break;
                case System.Windows.MessageBoxResult.OK:
                    break;
                case System.Windows.MessageBoxResult.Cancel:
                    JournalFileSystemController.AddInfo("Восстановление файловой системы отменено");
                    break;
                case System.Windows.MessageBoxResult.Yes:
                    this.ClearFileSystem();
                    this.backup.Restore(this.filesList);
                    foreach (var file in this.filesList)
                    {
                        this.Save(file);
                    }
                    JournalFileSystemController.AddInfo("Сессия файловой системы успешно восстановлена");
                    break;
                case System.Windows.MessageBoxResult.No:
                    JournalFileSystemController.AddInfo("Предыдущая версия файловой системы очищена");
                    this.ClearFileSystem();
                    this.backup.ClearFileSystem();
                    break;
                default:
                    break;
            }

            if (MessageBoxHellpers.Questions("Восстанвоить предыдущую сессию приложения ?") == System.Windows.MessageBoxResult.Yes)
            {
                if (this.backup.RestoreState(out state))
                {
                    JournalFileSystemController.AddInfo("Сессию приложения успешно установлена");
                    return true;
                }
                else
                {
                    JournalFileSystemController.AddInfo("Сессию приложения не удалось восстанвоить");
                    return false;
                }
            }
            else
            {
                JournalFileSystemController.AddInfo("Предыдущая сессия приложения отменена");
                state = null;
                return false;
            }
        }
    }
}
