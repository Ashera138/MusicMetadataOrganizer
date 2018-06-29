using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MusicMetadataUpdater_v2._0;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MusicMetadataUI
{
    public partial class UpdaterWindow : Window
    {
        public ObservableCollection<MetadataUpdate> Updates { get; set; }
        private ObservableCollection<MetadataFile> _files;

        public UpdaterWindow()
        {
            InitializeComponent();
            this.Show();

            Updates = new ObservableCollection<MetadataUpdate>();
            ProcessFiles();

            lvFiles.ItemsSource = Updates;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvFiles.ItemsSource);
            var groupDescription = new PropertyGroupDescription("MetadataFileName");
            view.GroupDescriptions.Add(groupDescription);
        }

        // vague name
        private void ProcessFiles()
        {
            var tempFiles = GetMetadataFilesFromSystemFiles(((MainWindow)Application.Current.MainWindow).userSelectedFiles).ToList();
            tempFiles.ForEach(f => f.PopulateUpdateList());
            _files = new ObservableCollection<MetadataFile>(tempFiles);

            foreach (var file in _files)
            {
                Updates.AddMany(file.Updates);
            }
        }

        private IEnumerable<MetadataFile> GetMetadataFilesFromSystemFiles(List<SystemFile> files)
        {
            foreach (SystemFile file in files)
            {
                yield return file.MetadataFile;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Not fixing this problem that it was supposed to fix: 
            // Could not copy "d:\my documents\visual studio 2017\Projects\MusicMetadataUpdater_v2.0\
            // MusicMetadataUpdater_v2.0\bin\Debug\MusicMetadataUpdater_v2.0.dll" to 
            // "bin\Debug\MusicMetadataUpdater_v2.0.dll".Beginning retry 10 in 1000ms.
            // The process cannot access the file 'bin\Debug\MusicMetadataUpdater_v2.0.dll' because it 
            // is being used by another process.The file is locked by: "MusicMetadataUI (12328)" 
            // MusicMetadataUI

            Application.Current.Shutdown();
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    lvFiles.Height = this.Height * .8;
        //}
    }
}
