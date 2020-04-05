using JournalingGUI.Hellpers;
using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace JournalingGUI.Controller
{
    /// <summary>
    /// Описывает структуру файловой системы для восстановления файлов
    /// </summary>
    public class BackupController: BaseFileSystem
    {
        /// <summary>
        /// Путь к файловой системе
        /// </summary>
        protected override string path { get; set; }

        /// <summary>
        /// Список backup файлов
        /// </summary>
        public ObservableCollection<FileModel> filesBackupList;

        public BackupController()
        {
            this.path = Path.Combine(Directory.GetCurrentDirectory(), "backup");
            this.filesBackupList = new ObservableCollection<FileModel>();
            this.Load();
        }
        
        ///<inheritdoc/>
        public override void Load()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(this.path);

                foreach (var file in directory.GetFiles())
                {
                    var fileModel = new FileModel()
                    {
                        body = FileSystemHellpers.GetBodyFile(file.FullName),
                        fileName = file.Name.Substring(0, file.Name.LastIndexOf('.')),
                        extension = file.Extension
                    };

                    this.filesBackupList.Add(fileModel);
                }
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error("Ошибка", exc.Message);
            }
        }

        ///<inheritdoc/>
        public override bool Save(FileModel file)
        {
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

        public bool SaveState(StateApplication state)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(state.GetType());

                using (StreamWriter sw = new StreamWriter(Path.Combine(this.path, StateApplication.fileName)))
                {
                    serializer.Serialize(sw, state);
                }
                return true;
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error($"Исключение {nameof(FileSystemController)}.SaveState", exc.Message);
            }

            return false;
        }

        /// <summary>
        /// Восстанавливает состояние приложение из xml файла состояний
        /// </summary>
        /// <param name="state"></param>
        public bool RestoreState(out StateApplication state)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StateApplication));

                using (StreamReader sr = new StreamReader(Path.Combine(this.path, StateApplication.fileName)))
                {
                    state = (StateApplication)serializer.Deserialize(sr);
                }
                return true;
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error($"Исключение {nameof(FileSystemController)}.SaveState", exc.Message);
            }

            state = null;

            return false;
        }

        /// <summary>
        /// Сохраняет все файлы в backup
        /// </summary>
        /// <param name="files">Файлы, котоыре нужно сохранить</param>
        public void SaveAll(ObservableCollection<FileModel> files)
        {
            foreach (var file in files)
            {
                if(file.fileName != FileModel.DefaultFileName)
                    this.Save(file);
            }
        }
    
        /// <summary>
        /// Восстанавливает файловую систему из последнего сеанса
        /// </summary>
        /// <param name="files">Список файлов в текущей файловой системе</param>
        public void Restore(ObservableCollection<FileModel> files)
        {
            foreach (var file in this.filesBackupList)
            {
                if(file.ToString() != StateApplication.fileName)
                    files.Add(file);
            }
        }
    }
}
