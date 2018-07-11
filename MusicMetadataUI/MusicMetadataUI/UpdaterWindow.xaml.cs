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

        public UpdaterWindow(ObservableCollection<MetadataUpdate> updates)
        {
            Updates = updates;
            InitializeComponent();
            this.Show();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Updates == null)
            {
                MessageBox.Show("ObservableCollection<MetadataUpdate> Updates is null " +
                "during Window_Loaded event.");
            }
            dgData.ItemsSource = Updates;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgData.ItemsSource);
            var groupDescription = new PropertyGroupDescription("MetadataFileName");
            view.GroupDescriptions.Add(groupDescription);
        }
    }
}
