using GalaSoft.MvvmLight.Command;
using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicMetadataUI
{
    public class FileViewModel// : INotifyPropertyChanged // implement this
    {
        public ObservableCollection<UpdateHelper> Updates { get; set; }

        public FileViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            // ***** populate updates with call to database or other

            //SaveCommand = new RelayCommand(OnSave);
        }

        //public event PropertyChangedEventHandler PropertyChanged = delegate { };

        //private MetadataFile _metadataFile;
        //public MetadataFile MetadataFile
        //{
        //    get { return _metadataFile; }
        //    set
        //    {
        //        if (!value.Equals(_metadataFile))
        //        {
        //            _metadataFile = value;
        //            PropertyChanged(this, new PropertyChangedEventArgs("MetadataFile"));
        //        }
        //    }
        //}

        //public /*async*/ void LoadFile()
        //{
        //    //MetadataFile = // database call with fileid or filepath
        //}

        //private /*async*/ void OnSave()
        //{
        //    //MetadataFile = await _repo.Update(MetadataFile);
        //}

        //public ICommand SaveCommand { get; set; }
    }
}
