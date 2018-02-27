using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    public interface IFile
    {
        string Filepath { get; set; }
        void Save();
        bool Equals(IFile file);
    }
}
