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

namespace MusicMetadataUI
{
    public partial class UpdaterWindow : Window
    {
        private MainWindow _startingWindow;
        private ObservableCollection<MusicMetadataUpdater_v2._0.MetadataFile> _files;

        public UpdaterWindow(MainWindow originalWindow)
        {
            InitializeComponent();
            this.Show();

            _startingWindow = originalWindow;
            var tempFiles = GetMetadataFilesFromSystemFiles(_startingWindow.files).ToList();
            tempFiles.ForEach(f => f.PopulateUpdateList());
            _files = new ObservableCollection<MusicMetadataUpdater_v2._0.MetadataFile>(tempFiles);
            dataGrid.ItemsSource = _files;
            //dataGrid.Visibility = Visibility.Hidden;

            /*
            var view = new GridView();
            view.Columns.Add(new GridViewColumn { Header = "File", DisplayMemberBinding = new Binding("File") });
            view.Columns.Add(new GridViewColumn { Header = "Field", DisplayMemberBinding = new Binding("Field") });
            view.Columns.Add(new GridViewColumn { Header = "Old Value", DisplayMemberBinding = new Binding("OldValue") });
            view.Columns.Add(new GridViewColumn { Header = "New Value", DisplayMemberBinding = new Binding("NewValue") });
            listView.View = view;
            listView.Items.Add(new { File = _files[0], Field = "TestField", OldValue = "Test old value", NewValue = "Test new value" });
            */
            
        }

        private IEnumerable<MusicMetadataUpdater_v2._0.MetadataFile> GetMetadataFilesFromSystemFiles(List<SystemFile> files)
        {
            foreach (SystemFile file in files)
            {
                yield return file.MetadataFile;
            }
        }



        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
