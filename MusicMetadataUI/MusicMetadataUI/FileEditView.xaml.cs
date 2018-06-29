using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicMetadataUI
{
    /// <summary>
    /// Interaction logic for FileEditView.xaml
    /// </summary>
    public partial class FileEditView : UserControl
    {
        public ObservableCollection<UpdateHelper> FilesToDisplay { get; set; }
        private ObservableCollection<MetadataFile> _files;

        public FileEditView()
        {
            InitializeComponent();

            FilesToDisplay = new ObservableCollection<UpdateHelper>();
            ProcessFiles();

            var viewModel = new FileViewModel() { Updates = FilesToDisplay };
            DataContext = viewModel;
        }

        // vague name
        private void ProcessFiles()
        {
            var tempFiles = GetMetadataFilesFromSystemFiles(((MainWindow)Application.Current.MainWindow).userSelectedFiles).ToList();
            tempFiles.ForEach(f => f.PopulateUpdateList());
            _files = new ObservableCollection<MetadataFile>(tempFiles);

            foreach (var file in _files)
            {
                FilesToDisplay.Add(new UpdateHelper(file));
            }
        }

        private IEnumerable<MetadataFile> GetMetadataFilesFromSystemFiles(List<SystemFile> files)
        {
            foreach (SystemFile file in files)
            {
                yield return file.MetadataFile;
            }
        }
    }
}
