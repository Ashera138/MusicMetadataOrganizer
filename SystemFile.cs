using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MusicMetadataUpdater_v2._0
{
    public class SystemFile : IFile
    {
        private string _filepath;
        [Key]
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
        public string FileName { get; set; }
        [StringLength(260)]
        public string FileDirectory { get; set; }
        [StringLength(10)]
        public string FileExtension { get; set; }
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
            FileName = SysIOFile.Name;
            FileDirectory = SysIOFile.DirectoryName;
            FileExtension = SysIOFile.Extension;
            CreationTime = Convert.ToDateTime(SysIOFile.CreationTime);
            LastAccessTime = Convert.ToDateTime(SysIOFile.LastAccessTime);
            LengthInBytes = SysIOFile.Length;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public bool Equals(IFile file)
        {
            bool isEqual = true;
            var systemFile = file as SystemFile;
            if (FileName != systemFile.FileName ||
                FileDirectory != systemFile.FileDirectory ||
                FileExtension != systemFile.FileExtension ||
                CreationTime != systemFile.CreationTime ||
                LastAccessTime != systemFile.LastAccessTime ||
                LengthInBytes != systemFile.LengthInBytes)
                    isEqual = false;
            return isEqual;
        }
    }
}
