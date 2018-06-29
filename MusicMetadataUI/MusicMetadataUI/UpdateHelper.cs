using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUI
{
    [Obsolete("Using a MetadataUpdate collection instead")]
    public class UpdateHelper : INotifyPropertyChanged // implement in property setters
    {
        public MetadataFile MetadataFile { get; private set; }
        public string Name { get; set; } = "Not set";
        public string Filepath { get; set; } = "Not set";
        public ObservableCollection<MetadataUpdate> Updates { get; set; }

        public UpdateHelper(MetadataFile metadataFile)
        {
            this.MetadataFile = metadataFile;
            this.Name = MetadataFile.ToString();
            this.Filepath = MetadataFile.Filepath;
            this.Updates = MetadataFile.Updates;
        }

        public UpdateHelper() { }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
