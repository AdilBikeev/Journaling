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

            this.filesListBox.ItemsSource = FileSystem.filesList;
        }
    }
}
