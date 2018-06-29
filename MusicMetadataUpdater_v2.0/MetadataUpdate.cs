namespace MusicMetadataUpdater_v2._0
{
    public class MetadataUpdate
    {
        // Maybe can eventually get rid of some of these props - might not use all
        public MetadataFile MetadataFile { get; set; }
        public string Filepath { get; set; }
        public string MetadataFileName { get; set; }
        public int FileId { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool UserConfirmedUpdate { get; set; }

        public MetadataUpdate(MetadataFile file, string field, string oldValue, string newValue)
        {
            MetadataFile = file;
            Filepath = file.Filepath;
            MetadataFileName = file.ToString(); // or file.Name
            FileId = file.MetadataFileId;
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public MetadataUpdate() { }
    }
}