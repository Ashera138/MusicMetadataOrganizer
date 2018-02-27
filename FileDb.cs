using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    class FileDb : DbContext
    {
        public DbSet<MetadataFile> TagLibFiles { get; set; }
        public DbSet<SystemFile> SysIOFiles { get; set; }
    }
}
