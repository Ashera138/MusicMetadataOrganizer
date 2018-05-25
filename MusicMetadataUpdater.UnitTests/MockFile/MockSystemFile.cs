using MusicMetadataUpdater_v2._0;
using System;
using System.Data;

namespace MusicMetadataUpdater.UnitTests.MockFile
{
    internal class MockSystemFile : IFile
    {
        public int SystemFileId { get; set; }
        public string Filepath { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public long LengthInBytes { get; set; }

        [Obsolete("Moving to TestClassFactory.")]
        internal DataTable GetMockSystemFileDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("SystemFileId");
            table.Columns.Add("Filepath");
            table.Columns.Add("Name");
            table.Columns.Add("Directory");
            table.Columns.Add("Extension");
            table.Columns.Add("CreationTime");
            table.Columns.Add("LastAccessTime");
            table.Columns.Add("LengthInBytes");

            table.Rows.Add(SystemFileId, Filepath, Name, Directory, 
                           Extension, CreationTime, LastAccessTime, LengthInBytes);
            return table;
        }

        public bool Equals(IFile otherFile)
        {
            bool isEqual = true;
            var systemFile = otherFile as MockSystemFile;
            if (Name != systemFile.Name ||
                Directory != systemFile.Directory ||
                Extension != systemFile.Extension ||
                CreationTime.Date != systemFile.CreationTime.Date ||
                LastAccessTime.Date != systemFile.LastAccessTime.Date ||
                LengthInBytes != systemFile.LengthInBytes)
                isEqual = false;
            return isEqual;
        }

        public bool TrySave() => false;

        public override string ToString() => Name;
    }
}
