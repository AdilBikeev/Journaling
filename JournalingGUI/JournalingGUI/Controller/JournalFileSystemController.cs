using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JournalingGUI.Controller
{
    /// <summary>
    /// Описывает журнал файловой системы
    /// </summary>
    public class JournalFileSystemController
    {
        /// <summary>
        /// Словарь удаленных файлов
        /// </summary>
        public Dictionary<int, FileModel> deleteFiles = new Dictionary<int, FileModel>();

        /// <summary>
        /// Словарь измененных файлов
        /// </summary>
        public Dictionary<int, FileModel> changeBodyFiles = new Dictionary<int, FileModel>();

        /// <summary>
        /// Список новых файлов
        /// </summary>
        public ObservableCollection<FileModel> filesListNew = new ObservableCollection<FileModel>();

        /// <summary>
        /// Обновляет данные файлы журнала на основе добавленного файла
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filesListOld"></param>
        public void Add(FileModel file, ObservableCollection<FileModel> filesListOld)
        {
            var itemFile = filesListOld.FirstOrDefault(x => x.fileName == file.fileName);

            // Если данного файла нет в списке ListBox
            if (itemFile == null)
            {
                filesListNew.Add(file);
            }else
            {
                var index = filesListOld.IndexOf(itemFile);

                //Если тело документа изменилось
                if(itemFile.body != file.body)
                {
                    changeBodyFiles.Add(index, file);
                }
            }
        }
    
        /// <summary>
        /// Проверяет какие файлы были удалены из файловой системы.
        /// </summary>
        /// <param name="filesListOld">Список файлов, которые были в файловой системе.</param>
        /// <param name="currentFiles">Список файлов, котоые есть в данный момент в файловой системе.</param>
        public void CheckDeleteFiles(ObservableCollection<FileModel> filesListOld, ObservableCollection<FileModel> currentFiles)
        {
            foreach (var file in filesListOld)
            {
                var item = currentFiles.FirstOrDefault(x => x.fileName == file.fileName);

                //Если файла в данный момент нет, но он был
                if (item == null)
                {
                    var index = filesListOld.IndexOf(file);
                    this.deleteFiles.Add(index, file);
                }
            }
        }
    }
}
