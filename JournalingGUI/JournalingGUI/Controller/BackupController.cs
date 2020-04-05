using JournalingGUI.Hellpers;
using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace JournalingGUI.Controller
{
    /// <summary>
    /// Описывает структуру файловой системы для восстановления файлов
    /// </summary>
    internal class BackupController: BaseFileSystem
    {
        /// <summary>
        /// Путь к файловой системе
        /// </summary>
        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "backup");

        public Backup backup;

        public BackupController()
        {
            this.backup = new Backup();
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

                    this.backup.filesBackupList.Add(fileModel);
                }
            }
            catch (Exception exc)
            {
                MessageBoxHellpers.Error("Ошибка", exc.Message);
            }
        }

        ///<inheritdoc/>
        public override void Save(FileModel file)
        {
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
    }
}
