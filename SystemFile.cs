using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MusicMetadataUpdater_v2._0
{
    public class SystemFile : IFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
        private string _filepath;
        public string Filepath
        {
            get
            {
                return _filepath;
            }
            set
            {
                if (!System.IO.File.Exists(value))
                    throw new IOException();
                _filepath = value;
            }
        }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(260)]
        public string Directory { get; set; }
        [StringLength(10)]
        public string Extension { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public long LengthInBytes { get; set; }
        private FileInfo SysIOFile { get; set; }

        private SystemFile()
        {
            SysIOFile = new FileInfo(Filepath);
            PopulateFields();
        }

        public SystemFile(string filepath)
        {
            this.Filepath = filepath;
            SysIOFile = new FileInfo(Filepath);
            PopulateFields();
        }

        private void PopulateFields()
        {
            Filepath = SysIOFile.FullName;
            Name = SysIOFile.Name;
            Directory = SysIOFile.DirectoryName;
            Extension = SysIOFile.Extension;
            CreationTime = Convert.ToDateTime(SysIOFile.CreationTime);
            LastAccessTime = Convert.ToDateTime(SysIOFile.LastAccessTime);
            LengthInBytes = SysIOFile.Length;
        }

        public void Save()
        {
            //FileManipulator.MoveToCorrectArtistLocation(this);
            //FileManipulator.MoveToCorrectAlbumLocation(this);
            //FileManipulator.RenameFile(this);
        }

        public bool Equals(IFile file)
        {
            bool isEqual = true;
            var systemFile = file as SystemFile;
            if (Name != systemFile.Name ||
                Directory != systemFile.Directory ||
                Extension != systemFile.Extension ||
                CreationTime != systemFile.CreationTime ||
                LastAccessTime != systemFile.LastAccessTime ||
                LengthInBytes != systemFile.LengthInBytes)
                    isEqual = false;
            return isEqual;
        }
    }
}
