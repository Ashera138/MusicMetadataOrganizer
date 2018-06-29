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

        public MainWindow()
        {
            InitializeComponent();
            userSelectedFiles = new List<SystemFile>();
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
                userSelectedFiles = await Task.Run(() => FileSearcher.ExtractFiles(directory));
                FilterFilesAgainstDatabaseRecords();
                StopBusyIndicator();
                PopulateDetailsTextBoxWithFiles();
            }

            viewBlacklistedSongsButton.IsEnabled = true;
            checkMetadataButton.IsEnabled = true;
            saveButton.IsEnabled = false;
        }

        private void FilterFilesAgainstDatabaseRecords()
        {
            // filter query result against _files
            foreach (var file in userSelectedFiles)
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

        // Holy crap this code is awful
        private async void CheckMetadataButton_Click(object sender, RoutedEventArgs e)
        {
            headerTextBlock.Text = "Checking for updated metadata, please wait...";
            StartBusyIndicator();

            foreach (var file in userSelectedFiles.Where(f => f.MetadataFile.CheckForUpdates == true))
            {
                file.MetadataFile.Response = await GracenoteAPI.QueryAsync(file.MetadataFile);
            }

            var systemFilesPendingUpdates = userSelectedFiles.Where(f => f.MetadataFile.IsUpdateNeeded() == true);

            bool updateNeeded = systemFilesPendingUpdates.Any();
            if (updateNeeded)
            {
                var metadataFilesPendingUpdates = systemFilesPendingUpdates.Select(f => f.MetadataFile).Cast<MetadataFile>().ToList();
                var allUpdates = ExtractUpdates(metadataFilesPendingUpdates);
                var updatesAsUpdateHelperObj = Convert(allUpdates);

                this.Visibility = Visibility.Collapsed;
                var updaterWindow = new UpdaterWindow
                {
                    DataContext = new FileViewModel() { Updates = updatesAsUpdateHelperObj }
                };
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

        [Obsolete("No longer using the FileEditView so I don't need this")]
        private ObservableCollection<UpdateHelper> Convert(ObservableCollection<MetadataUpdate> updates)
        {
            var convertedUpdates = new ObservableCollection<UpdateHelper>();
            foreach (MetadataUpdate update in updates)
            {
                convertedUpdates.Add(new UpdateHelper(update.MetadataFile));
            }
            return convertedUpdates;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // sample code for saving changes to the database
            // _metadataRepository.UpdateMetadataFileAsync(_file);
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
