using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JournalingGUI.Controller;
using JournalingGUI.Hellpers;
using JournalingGUI.Models;

namespace JournalingGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileSystemController FileSystem;

        public MainWindow()
        {
            InitializeComponent();
            FileSystem = new FileSystemController();
            FileSystem.UpdateFileSystemAsync();

            this.FilesListBox.ItemsSource = FileSystem.filesList;
            InitStateForm();
        }

        private void InitStateForm()
        {
            this.NameFileTb.IsEnabled = false;
            this.BodyFileRtb.IsEnabled = false;
            this.SaveFileBtn.IsEnabled = false;
            this.DeleteFileDtn.IsEnabled = false;
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.BodyFileRtb.IsEnabled = true;
            this.SaveFileBtn.IsEnabled = true;

            if(this.FilesListBox.SelectedItem != null)
            {
                var item = (FileModel)this.FilesListBox.SelectedItem;

                if(item.fileName != FileModel.DefaultFileName)
                {
                    this.NameFileTb.IsEnabled = false;
                    this.DeleteFileDtn.IsEnabled = true;
                    this.NameFileTb.Text = item.fileName;
                }else
                {
                    this.NameFileTb.IsEnabled = true;
                    this.DeleteFileDtn.IsEnabled = false;
                    this.NameFileTb.Text = string.Empty;
                }

                TextRange textRange = new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd);
                textRange.Text = item.body;
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.FilesListBox.SelectedItem != null)
            {
                FileModel file = new FileModel();
                bool isNewFile = file.fileName == FileModel.DefaultFileName;

                file.fileName = this.NameFileTb.Text;
                if (!string.IsNullOrEmpty(file.fileName))
                {
                    if (file.fileName != FileModel.DefaultFileName)
                    {
                        file.body = (new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd)).Text;
                        FileSystem.Save(file, isNewFile);
                    }
                    else
                    {
                        MessageBoxHellpers.Error("Ошибка", "Недопустимое имя файла");
                    }
                }
                else
                {
                    MessageBoxHellpers.Error("Ошибка", "Поле 'Имя файла' обязательно для заполнения !");
                }
            }
        }

        private void DeleteFileDtn_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.NameFileTb.Text;
            var items = this.FilesListBox.Items;
            this.FilesListBox.SelectedItem = items[items.Count-2];
            Thread.Sleep(1000);
            FileSystem.Delete(fileName);
        }
    }
}
