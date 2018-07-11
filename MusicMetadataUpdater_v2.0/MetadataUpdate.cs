using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicMetadataUpdater_v2._0
{
    public class MetadataUpdate : INotifyPropertyChanged, IEditableObject
    {
        public MetadataFile MetadataFile { get; set; }

        public string Filepath { get; private set; }

        public string MetadataFileName { get; private set; }

        public string Field { get; private set; }

        public string OldValue { get; set; }

        private string _newValue;
        public string NewValue
        {
            get { return _newValue; }
            set
            {
                _newValue = value;
                RaisePropertyChanged();
            }
        }

        private bool _userConfirmedUpdate;
        public bool UserConfirmedUpdate
        {
            get { return _userConfirmedUpdate; }
            set
            {
                _userConfirmedUpdate = value;
                RaisePropertyChanged();
            }
        }

        public MetadataUpdate(MetadataFile file, string field, string oldValue, string newValue)
        {
            MetadataFile = file;
            Filepath = file.Filepath;
            MetadataFileName = file.ToString();
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public MetadataUpdate() { }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        private MetadataUpdate _backupCopy;
        private bool _inEdit;

        // changes start being entered by user
        public void BeginEdit()
        {
            if (_inEdit) return;
            _inEdit = true;
            _backupCopy = this.MemberwiseClone() as MetadataUpdate;
        }

        // user clicks confirm button
        public void EndEdit()
        {
            if (!_inEdit) return;
            _inEdit = false;
            _backupCopy = null;
        }

        // user clicks cancel button
        public void CancelEdit()
        {
            if (!_inEdit) return;
            _inEdit = false;
            this.NewValue = _backupCopy.NewValue;
            this.UserConfirmedUpdate = _backupCopy.UserConfirmedUpdate;
        }
    }
}