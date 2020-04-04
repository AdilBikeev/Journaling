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
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (FileModel)this.FilesListBox.SelectedItem;
            this.NameFileLb.Content = item.fileName;
            TextRange textRange = new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd);
            textRange.Text = item.body;
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            FileSystem.Save(this.NameFileLb.Content.ToString(), (new TextRange(this.BodyFileRtb.Document.ContentStart, this.BodyFileRtb.Document.ContentEnd)).Text);
        }
    }
}
