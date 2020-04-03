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
        /// Обновляет ListBox на основе Журнала.
        /// </summary>
        /// <param name="filesListOld">Старый список.</param>
        /// <param name="journal">Журнал файловой системы.</param>
        public static void UpdateFileList(ObservableCollection<FileModel> filesListOld, JournalFileSystemController journal)
        {
            //Удаляем из ListBox удаленные из файловой системы файлы
            foreach (var file in journal.deleteFiles)
            {
                var item = filesListOld.FirstOrDefault(x => x.fileName == file.Value.fileName);
                filesListOld.Remove(item);
            }

            //Обновляем тела документов
            foreach (var file in journal.changeBodyFiles)
            {
                var item = filesListOld.FirstOrDefault(x => x.fileName == file.Value.fileName);
                item.body = file.Value.body;
            }

            //Добавляем новые документы
            foreach (var file in journal.filesListNew)
            {
                filesListOld.Add(file);
            }
        }
    }
}
