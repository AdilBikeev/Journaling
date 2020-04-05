using JournalingGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JournalingGUI.Models
{
    class Backup
    {
        /// <summary>
        /// Список backup файлов
        /// </summary>
        public ObservableCollection<FileModel> filesBackupList = new ObservableCollection<FileModel>();
    }
}
