using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;

namespace MusicMetadataUpdater_v2._0
{
    public class SystemFile : IFile
    {
        public int SystemFileId { get; set; }
        public virtual MetadataFile MetadataFile { get; set; }
        private string _filepath;
        [StringLength(260)]
        public string Filepath
        {
            get
            {
                return _filepath;
            }
            set
            {
                if (!File.Exists(value))
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
            MetadataFile = new MetadataFile(Filepath);
        }

        public bool TrySave()
        {
            bool success;
            try
            {
                if (MetadataFile == null)
                    throw new ArgumentNullException("Attempted to save SystemFile when its MetadataFile property is null.");
                MoveToCorrectArtistLocation();
                MoveToCorrectAlbumLocation();
                RenameFile();
                success = true;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"SystemFile.TrySave() - " +
                    $"Can not save SystemFile to '{Filepath}'. {ex.GetType()}: \"{ex.Message}\"");
                success = false;
            }
            return success;
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

        public void MoveToCorrectArtistLocation()
        {
            Regex artistDirectoryRegex = new Regex(@"([^\\]+)\\([^\\]+)\\([^\\]+)$");
            RenameDirectory(artistDirectoryRegex, MetadataFile.Artist);
        }

        public void MoveToCorrectAlbumLocation()
        {
            Regex albumDirectoryRegex = new Regex(@"([^\\]+)\\([^\\]+)$");
            RenameDirectory(albumDirectoryRegex, MetadataFile.Album);
        }

        private void RenameDirectory(Regex directoryRegex, string newName)
        {
            string newDirectory = CreateSanitizedDirectoryName(directoryRegex, newName);
            DirectoryInfo currentDirectory = FileManipulator.GetDirectoryInfo(this.Directory);

            if (this.Directory != newDirectory)
            {
                if (this.Directory.EqualsIgnoreCase(newDirectory))
                {
                    RenameDirectoryAsTemp(currentDirectory);
                    currentDirectory = FileManipulator.GetDirectoryInfo(this.Directory);
                }
                RenameFolder(currentDirectory, newDirectory);
            }
            FileManipulator.DeleteEmptyFolders(currentDirectory);
        }

        private string CreateSanitizedDirectoryName(Regex directoryRegex, string newName)
        {
            Group directoryRegexGroup = directoryRegex.Match(this.Filepath).Groups[1];
            string sanitizedNewName = StringCleaner.RemoveInvalidDirectoryCharacters(newName);
            string newDirectory = directoryRegexGroup.Replace(this.Directory, sanitizedNewName);
            return newDirectory;
        }

        private void RenameDirectoryAsTemp(DirectoryInfo currentDirectory)
        {
            var tempPath = currentDirectory.FullName.Replace(currentDirectory.Name, @"_temp\");
            currentDirectory.MoveTo(tempPath);
            this.Directory = tempPath;
            this.Filepath = Path.Combine(tempPath, this.Name);
        }

        private void RenameFolder(DirectoryInfo currentDirectory, string destDirectory)
        {
            try
            {
                FileManipulator.CreateDirectory(destDirectory);
                MoveFileAfterDirectoryRename(destDirectory);
            }
            catch (Exception ex)
            {
                LogWriter.Write($"FileManipulator.RenameFolder - Can not rename (move) '{currentDirectory.FullName}' " +
                        $"to '{destDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private void MoveFileAfterDirectoryRename(string destDirectory)
        {
            var newFilepath = this.Filepath.Replace(this.Directory, destDirectory);
            File.Move(this.Filepath, newFilepath);
            this.Filepath = newFilepath;
            this.Directory = destDirectory;
        }

        public void RenameFile()
        {
            var newFileName = CreateSanitizedFileName(MetadataFile.Title);
            if (this.Name != newFileName)
            {
                if (TryRenameFile(newFileName))
                {
                    this.Name = newFileName;
                    this.Filepath = Path.Combine(this.Directory, this.Name);
                }
            }
        }

        private string CreateSanitizedFileName(string newName)
        {
            var newFileName = newName + this.Extension;
            var validFileName = StringCleaner.RemoveInvalidFileNameCharacters(newFileName);
            return validFileName;
        }

        private bool TryRenameFile(string newFileName)
        {
            bool success;
            var fileInfo = FileManipulator.GetFileInfo(this.Filepath);
            var currentFileName = fileInfo.Name;
            var destPath = this.Filepath.Replace(currentFileName, newFileName);

            try
            {
                if (currentFileName.EqualsIgnoreCase(newFileName))
                    RenameFileAsTemp(fileInfo);
                fileInfo.MoveTo(destPath);
                success = true;
            }

            catch (Exception ex)
            {
                LogWriter.Write($"FileManipulator.RenameFile() - Can not rename (move) " +
                    $"'{this.Filepath}' to '{destPath}'. {ex.GetType()}: \"{ex.Message}\"");
                success = false;
            }
            return success;
        }

        private void RenameFileAsTemp(FileInfo currentFile)
        {
            var tempPath = currentFile.FullName.Replace(currentFile.Name, @"_temp");
            currentFile.MoveTo(tempPath);
            this.Filepath = tempPath;
        }

        public override string ToString()
        {
            return $"{MetadataFile.Artist} - {MetadataFile.Title}";
        }
    }
}
