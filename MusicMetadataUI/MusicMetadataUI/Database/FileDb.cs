using System.Data.Entity;

namespace MusicMetadataUpdater_v2._0
{
    public class FileDb : DbContext
    {
        public DbSet<MetadataFile> MetadataFiles { get; set; }
        public DbSet<SystemFile> SystemFiles { get; set; }
    }
}
