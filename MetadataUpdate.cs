namespace MusicMetadataUpdater_v2._0
{
    public class MetadataUpdate
    {
        public int FileId { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public MetadataUpdate(int fileId, string field, string oldValue, string newValue)
        {
            this.FileId = fileId;
            this.Field = field;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}