using System.Data.Entity;

namespace MusicMetadataUpdater_v2._0
{
    class FileDb : DbContext
    {
        public DbSet<MetadataFile> TagLibFiles { get; set; }
        public DbSet<SystemFile> SysIOFiles { get; set; }
    }
}
