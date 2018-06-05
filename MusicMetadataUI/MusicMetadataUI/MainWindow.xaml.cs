using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MusicMetadataUI.Database;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUI
{
    public partial class MainWindow : Window
    {
        internal List<SystemFile> files;
        FileRepository _fileRepository;
        List<MetadataFile> _allMetadataFileRecords;

        public MainWindow()
        {
            InitializeComponent();
            files = new List<SystemFile>();
            headerTextBlock.Text = "Select some files to start.";
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            _fileRepository = new FileRepository();
            var systemFiles = await _fileRepository.GetFilesAsync();
            _allMetadataFileRecords = await _fileRepository.GetMetadataFilesAsync();
            // DataContext = _allMetadataFileRecords;
        }



        private async void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var directory = FileSearcher.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                detailsTextBox.Text = "Grabbing files...";
                StartBusyIndicator();
                files = await Task.Run(() => FileSearcher.ExtractFiles(directory));
                FilterFilesAgainstDatabaseRecords();
                StopBusyIndicator();
                PopulateDetailsTextBoxWithFiles();
            }

            viewBlacklistedSongsButton.IsEnabled = true;
            checkMetadataButton.IsEnabled = true;
            saveButton.IsEnabled = false;
        }

        // split this method
        private void FilterFilesAgainstDatabaseRecords()
        {
            // filter query result against _files
            foreach (var file in files)
            {
                if (HasMatchInDatabase(file.MetadataFile))
                    file.MetadataFile.CheckForUpdates = false;
            }
        }

        // consider putting this and similar logic in a database class
        private bool HasMatchInDatabase(MetadataFile metadatafile)
        {
            foreach (MetadataFile metadataFileRecord in _allMetadataFileRecords)
            {
                if (metadataFileRecord.Equals(metadatafile))
                    return true;
            }
            return false;
        }

        private void PopulateDetailsTextBoxWithFiles()
        {
            detailsTextBox.Clear();
            for (int i = 0; i < files.Count; i++)
            {
                if (i == files.Count - 1)
                    detailsTextBox.Text += files[i];
                else
                    detailsTextBox.Text += files[i] + Environment.NewLine;
            }
        }

        private void ViewBlacklistedSongsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        private async void CheckMetadataButton_Click(object sender, RoutedEventArgs e)
        {
            headerTextBlock.Text = "Checking for updated metadata, please wait...";
            StartBusyIndicator();

            foreach (var file in files.Where(f => f.MetadataFile.CheckForUpdates == true))
            {
                file.MetadataFile.Response = await GracenoteAPI.QueryAsync(file.MetadataFile);
            }

            // Check this if statement logic
            bool updateNeeded = files.Where(f => f.MetadataFile.IsUpdateNeeded() == true).Any();
            if (updateNeeded)
            {
                this.Visibility = Visibility.Collapsed;
                var updaterWindow = new UpdaterWindow(this);
            }
            else
            {
                headerTextBlock.Text = "All songs are up to date.";
            }
            StopBusyIndicator();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // sample code for saving changes to the database
            // _metadataRepository.UpdateMetadataFileAsync(_file);
            MessageBox.Show("Not yet implemented.");
        }

        private void StartBusyIndicator()
        {
            busyIndicator.IsEnabled = true;
            busyIndicator.Visibility = Visibility.Visible;
        }

        private void StopBusyIndicator()
        {
            busyIndicator.IsEnabled = false;
            busyIndicator.Visibility = Visibility.Hidden;
        }
    }
}
