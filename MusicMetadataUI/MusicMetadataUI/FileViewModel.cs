using GalaSoft.MvvmLight.Command;
using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicMetadataUI
{
    // TODO: Flush this class out. Design ideas are written on my graph paper
    // Eventually write unit tests for this class
    public class FileViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MetadataUpdate> Updates { get; set; }

        public FileViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            // ***** populate updates with call to database or other

            //SaveCommand = new RelayCommand(OnSave);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public ObservableCollection<MetadataFile> MetadataFiles { get; private set; }

        //public /*async*/ void LoadFile()
        //{
        //    //MetadataFile = // database call with fileid or filepath
        //}

        //private /*async*/ void OnSave()
        //{
        //    //MetadataFile = await _repo.Update(MetadataFile);
        //}

        public ICommand SaveCommand { get; set; }
    }
}
