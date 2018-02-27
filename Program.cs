using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    class Program
    {
        static FileDb _db = new FileDb();

        static void Main(string[] args)
        {
            var tglbFiles = _db.TagLibFiles.ToList();
            foreach (var item in tglbFiles)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
