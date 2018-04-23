namespace MusicMetadataUpdater_v2._0
{
    public interface IFile
    {
        //int FileId { get; set; }
        string Filepath { get; set; }
        bool TrySave();
        bool Equals(IFile file);
    }
}
