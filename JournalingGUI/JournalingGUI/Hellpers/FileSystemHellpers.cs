using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using JournalingGUI.Models;
using JournalingGUI.Controller;
using System.Linq;

namespace JournalingGUI.Hellpers
{
    public static class FileSystemHellpers
    {
        private static object locker = new object();

        /// <summary>
        /// Возвращает содержимое файла.
        /// </summary>
        /// <param name="path">Полный путь к файлу.</param>
        /// <returns></returns>
        public static string GetBodyFile(string path)
        {
            var body = string.Empty;

            using(StreamReader sr = new StreamReader(path))
            {
                body = sr.ReadToEnd();
            }

            return body;
        }

        /// <summary>
        /// Задает оригинальное имя для файла
        /// </summary>
        /// <param name="path">Путь, в котором проверяется оригинальность файла</param>
        /// <param name="file">Объект файла</param>
        /// <returns></returns>
        public static void SetOriginalFileName(string path, FileModel file)
        {
            var fileNameNew = $"{file.fileName} — копия";
            var countCopy = 0;
            while (File.Exists(Path.Combine(path, $"{fileNameNew}{file.extension}")))
            {
                countCopy = countCopy == 0 ? 2 : countCopy + 1;
                fileNameNew = $"{file.fileName} — копия ({countCopy})";
            }

            file.fileName = fileNameNew;
        }

        /// <summary>
        /// Обновляет ListBox на основе Журнала.
        /// </summary>
        /// <param name="filesListOld">Старый список.</param>
        /// <param name="journal">Журнал файловой системы.</param>
        public static void UpdateFileList(ObservableCollection<FileModel> filesListOld, JournalFileSystemController journal)
        {
            if (filesListOld.FirstOrDefault(x => x.fileName == FileModel.DefaultFileName) == null)
            {
                filesListOld.Add(new FileModel());
            }

            //Удаляем из ListBox удаленные из файловой системы файлы
            foreach (var file in journal.deleteFiles)
            {
                if(file.Value.fileName != FileModel.DefaultFileName)
                {
                    var item = filesListOld.FirstOrDefault(x => x.fileName == file.Value.fileName);
                    JournalFileSystemController.AddInfo($"Файл {item.ToString()} удален");
                    filesListOld.Remove(item);
                }
            }

            //Обновляем тела документов
            foreach (var file in journal.changeBodyFiles)
            {
                var item = filesListOld.FirstOrDefault(x => x.fileName == file.Value.fileName);
                if(item.body != file.Value.body)
                {
                    JournalFileSystemController.AddInfo($"Содержание файла {item.ToString()} изменено");
                }

                item.body = file.Value.body;
            }

            //Добавляем новые документы
            foreach (var file in journal.filesListNew)
            {
                filesListOld.Add(file);
                JournalFileSystemController.AddInfo($"Добавлен новый файл {file.ToString()}");
            }
        }
    }
}
