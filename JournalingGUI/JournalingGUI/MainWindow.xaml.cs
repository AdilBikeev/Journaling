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
            InitStateForm();
            InitJournal();

            FileSystem = new FileSystemController();
            this.RequestRestore();

            this.FilesListBox.ItemsSource = FileSystem.filesList;

            FileSystem.UpdateFileSystemAsync();
        }

        private void InitJournal()
        {
            JournalFileSystemController.journal = this.JournalRtb;
        }

        private void InitStateForm()
        {
            this.NameFileTb.IsEnabled = false;
            this.BodyFileRtb.IsEnabled = false;
            this.SaveFileBtn.IsEnabled = false;
            this.DeleteFileDtn.IsEnabled = false;
        }

        /// <summary>
        /// Сохраняет состояние приложения
        /// </summary>
        private void SaveState()
        {
            StateApplication state = new StateApplication
            {
                file = new FileModel()
                {
                    fileName = this.NameFileTb.Text,
                    body = (new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd)).Text
                },
                IsNewFile = this.NameFileTb.IsEnabled,
                GeneralLog = (new TextRange(this.JournalRtb.Document.ContentStart, this.JournalRtb.Document.ContentEnd)).Text
            };

            this.FileSystem.SaveState(state);
        }

        private void RequestRestore()
        {
            StateApplication state;
            if(this.FileSystem.RestoreFileSystem(out state))
            {
                if(state.IsNewFile)
                {
                    this.NameFileTb.IsEnabled = true;
                    this.DeleteFileDtn.IsEnabled = false;

                }else
                {
                    this.NameFileTb.IsEnabled = false;
                    this.DeleteFileDtn.IsEnabled = true;
                }

                this.BodyFileRtb.IsEnabled = true;
                (new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd)).Text = state.file.body;
                this.NameFileTb.Text = state.file.fileName;
            }
           (new TextRange(this.JournalRtb.Document.ContentStart, this.JournalRtb.Document.ContentEnd)).Text += state.GeneralLog;
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
                    JournalFileSystemController.AddInfo("Начало изменения существуещего файла");
                }
                else
                {
                    this.NameFileTb.IsEnabled = true;
                    this.DeleteFileDtn.IsEnabled = false;
                    this.NameFileTb.Text = string.Empty;
                    JournalFileSystemController.AddInfo("Начало создание нового файла");
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

                file.fileName = this.NameFileTb.Text;
                if (!string.IsNullOrEmpty(file.fileName))
                {
                    if (file.fileName != FileModel.DefaultFileName)
                    {
                        file.body = (new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd)).Text;
                        if(FileSystem.Save(file))
                        {
                            JournalFileSystemController.AddInfo($"Файл {file.ToString()} успешно сохранён");
                        }else
                        {
                            JournalFileSystemController.AddInfo($"Файл {file.ToString()} не сохранён");
                        }
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
            FileModel file = new FileModel();

            file.fileName = this.NameFileTb.Text;

            var items = this.FilesListBox.Items;
            this.FilesListBox.SelectedItem = items[0];
            Thread.Sleep(1000);
            FileSystem.Delete(file);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.FileSystem.SaveSession();
            this.SaveState();
        }
    }
}
