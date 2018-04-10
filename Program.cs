using System;
using System.Linq;

namespace MusicMetadataUpdater_v2._0
{
    class Program
    {
        static FileDb _db = new FileDb();

        static void Main(string[] args)
        {
            var metadataFiles = _db.TagLibFiles.ToList();
            foreach (var item in metadataFiles)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
