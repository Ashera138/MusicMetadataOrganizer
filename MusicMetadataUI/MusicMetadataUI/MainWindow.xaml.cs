using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MusicMetadataUI.Database;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUI
{
    public partial class MainWindow : Window
    {
        internal List<SystemFile> userSelectedFiles;
        FileRepository _fileRepository;
        List<MetadataFile> _allMetadataFileRecords;
        ObservableCollection<MetadataUpdate> _allUpdates;

        public MainWindow()
        {
            InitializeComponent();
            userSelectedFiles = new List<SystemFile>();
            _allUpdates = new ObservableCollection<MetadataUpdate>();
            headerTextBlock.Text = "Select some files to start.";
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            _fileRepository = new FileRepository();
            var systemFiles = await _fileRepository.GetFilesAsync();
            _allMetadataFileRecords = await _fileRepository.GetMetadataFilesAsync();
        }

        private async void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var directory = FileSearcher.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                detailsTextBox.Text = "Grabbing files...";
                StartBusyIndicator();
                userSelectedFiles = await Task.Run(() => FileSearcher.ExtractFiles(directory));
                await FilterFilesAgainstDatabaseRecords();
                StopBusyIndicator();
                PopulateDetailsTextBoxWithFiles();
            }

            viewBlacklistedSongsButton.IsEnabled = true;
            checkMetadataButton.IsEnabled = true;
            saveButton.IsEnabled = false;
        }

        private async Task FilterFilesAgainstDatabaseRecords()
        {
            foreach (var file in userSelectedFiles)
            {
                if (await _fileRepository.ContainsAsync(file.MetadataFile))
                    file.MetadataFile.CheckForUpdates = false;
            }
        }

        private void PopulateDetailsTextBoxWithFiles()
        {
            detailsTextBox.Clear();
            if (userSelectedFiles.Count == 0)
                detailsTextBox.Text = "No files to display.";
            for (int i = 0; i < userSelectedFiles.Count; i++)
            {
                if (i == userSelectedFiles.Count - 1)
                    detailsTextBox.Text += userSelectedFiles[i];
                else
                    detailsTextBox.Text += userSelectedFiles[i] + Environment.NewLine;
            }
        }

        private void ViewBlacklistedSongsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented.");
        }

        // Break this apart and move functionality to other classes if it seems fitting
        private async void CheckMetadataButton_Click(object sender, RoutedEventArgs e)
        {
            headerTextBlock.Text = "Checking for updated metadata, please wait...";
            StartBusyIndicator();

            // populates the API response field for each metadata file that is flagged for update
            foreach (var file in userSelectedFiles.Where(f => f.MetadataFile.CheckForUpdates == true))
            {
                file.MetadataFile.Response = await GracenoteAPI.QueryAsync(file.MetadataFile);
            }

            // filters all userSelectedFiles to only those where an update is needed
            var systemFilesPendingUpdates = userSelectedFiles.Where(f => f.MetadataFile.IsUpdateNeeded() == true);

            // checks to see if there are any updates needed
            bool updateNeeded = systemFilesPendingUpdates.Any();
            if (updateNeeded)
            {
                var metadataFilesPendingUpdates = systemFilesPendingUpdates.Select(f => f.MetadataFile).Cast<MetadataFile>().ToList();
                metadataFilesPendingUpdates.ForEach(f => f.PopulateUpdateList());
                _allUpdates = ExtractUpdates(metadataFilesPendingUpdates);

                
                var updaterWindow = new UpdaterWindow(_allUpdates);
            }
            else
            {
                headerTextBlock.Text = "All songs are up to date.";
            }
            StopBusyIndicator();
        }

        private ObservableCollection<MetadataUpdate> ExtractUpdates(List<MetadataFile> files)
        {
            var updates = new ObservableCollection<MetadataUpdate>();
            foreach (MetadataFile file in files)
            {
                updates.AddMany(file.Updates);
            }
            return updates;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // sample code for saving changes to the database
            // _metadataRepository.UpdateMetadataFileAsync(_file);
            // save file to disk
            MessageBox.Show("Not yet implemented.");
        }

        // Eventually move this code into XAML
        private void StartBusyIndicator()
        {
            busyIndicator.IsEnabled = true;
            busyIndicator.Visibility = Visibility.Visible;
        }

        // Eventually move this code into XAML
        private void StopBusyIndicator()
        {
            busyIndicator.IsEnabled = false;
            busyIndicator.Visibility = Visibility.Hidden;
        }
    }
}
