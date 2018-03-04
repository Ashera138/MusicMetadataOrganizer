using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicMetadataUpdater_v2._0
{
    public class FileManipulator
    {
        private SystemFile _systemFile;

        public FileManipulator(SystemFile systemFile)
        {
            _systemFile = systemFile;
        }

        public void MoveToCorrectArtistLocation(string songArtist)
        {
            Regex artistDirectoryRegex = new Regex(@"([^\\]+)\\([^\\]+)\\([^\\]+)$");
            RenameDirectory(artistDirectoryRegex, songArtist);
        }

        public void MoveToCorrectAlbumLocation(string songAlbum)
        {
            Regex albumDirectoryRegex = new Regex(@"([^\\]+)\\([^\\]+)$");
            RenameDirectory(albumDirectoryRegex, songAlbum);
        }

        private void RenameDirectory(Regex directoryRegex, string newName)
        {
            Group directoryRegexGroup = directoryRegex.Match(_systemFile.Filepath).Groups[1];
            string sanitizedNewName = StringCleaner.RemoveInvalidDirectoryCharacters(newName);
            string newDirectory = directoryRegexGroup.Replace(_systemFile.Directory, sanitizedNewName + "\\");
            DirectoryInfo currentDirectory = GetDirectoryInfo(_systemFile.Directory);

            if (_systemFile.Directory != newDirectory)
            {
                if (NamesAreEqualIgnoringCase(_systemFile.Directory, newDirectory))
                {
                    RenameDirectoryAsTemp(currentDirectory);
                    currentDirectory = GetDirectoryInfo(_systemFile.Directory);
                }
                RenameFolder(currentDirectory, newDirectory);
            }
            // See why I can't take out this variable re-initialization
            DeleteEmptyFolders(GetDirectoryInfo(_systemFile.Directory));
        }

        private bool NamesAreEqualIgnoringCase(string oldName, string newName)
        {
            return oldName.Equals(newName, StringComparison.OrdinalIgnoreCase);
        }

        private void RenameFolder(DirectoryInfo currentDirectory, string destDirectory)
        {
            try
            {
                CreateDirectory(destDirectory);
                MoveFileAfterDirectoryRename(destDirectory);
            }
            catch (Exception ex)
            {
                LogWriter.Write($"FileManipulator.RenameFolder - Can not rename (move) '{currentDirectory.FullName}' " +
                        $"to '{destDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private DirectoryInfo GetDirectoryInfo(string directory)
        {
            DirectoryInfo directoryInfo;
            if (Directory.Exists(directory))
                directoryInfo = new DirectoryInfo(directory);
            else
                throw new IOException($"FileManipulator.GetDirectoryInfo(): Cannot create a " +
                    $"DirectoryInfo object from '{directory}'. It does not exist.");
            return directoryInfo;
        }

        private void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private void RenameDirectoryAsTemp(DirectoryInfo currentDirectory)
        {
            var tempPath = currentDirectory.FullName.Replace(currentDirectory.Name, @"_temp\");
            currentDirectory.MoveTo(tempPath);
            _systemFile.Directory = tempPath;
            _systemFile.Filepath = Path.Combine(tempPath, _systemFile.Name);
        }

        private void MoveFileAfterDirectoryRename(string destDirectory)
        {
            var newFilepath = _systemFile.Filepath.Replace(_systemFile.Directory, destDirectory);
            File.Move(_systemFile.Filepath, newFilepath);
            _systemFile.Filepath = newFilepath;
            _systemFile.Directory = destDirectory;
        }

        // Refactor more
        private void DeleteEmptyFolders(DirectoryInfo folder)
        {
            if (!HasFiles(folder))
            {
                try
                {
                    if (HasFiles(folder.Parent))
                    {
                        folder.Delete();
                        return;
                    }

                    if (!FilesExistInSubdirectories(folder.Parent))
                    {
                        folder.Parent.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Write($"FileManipulator.DeleteEmptyParentFolder() - Can not delete " +
                        $"'{folder.Parent.FullName}'. {ex.GetType()}: \"{ex.Message}\"");
                }
            }
        }

        private bool HasFiles(DirectoryInfo folder)
        {
            var databaseFileExtension = ".db";
            List<FileInfo> files = folder.EnumerateFiles().Where(f => f.Extension != databaseFileExtension).ToList();
            return files.Count() > 0 ? true : false;
        }

        private bool FilesExistInSubdirectories(DirectoryInfo folder)
        {
            bool hasFiles = true;
            foreach (DirectoryInfo directory in folder.EnumerateDirectories())
            {
                if (HasFiles(directory))
                    hasFiles = false;
            }
            return hasFiles;
        }

        // Potentially rename
        public void RenameFile(string songTitle)
        {
            var currentFileName = _systemFile.Name;
            var newFileName = songTitle + _systemFile.Extension;
            var validFileName = StringCleaner.RemoveInvalidFileNameCharacters(newFileName);
            if (currentFileName != validFileName)
            {
                RenameFile(_systemFile.Filepath, validFileName);
                _systemFile.Name = validFileName;
                _systemFile.Filepath = Path.Combine(_systemFile.Directory, _systemFile.Name);  
            }
        }

        // Potentially rename
        private void RenameFile(string filepath, string newFileName)
        {
            var fileInfo = GetFileInfo(filepath);
            var currentFileName = fileInfo.Name;
            var destPath = filepath.Replace(currentFileName, newFileName);
            try
            {
                if (NamesAreEqualIgnoringCase(currentFileName, newFileName))
                    RenameFileAsTemp(fileInfo);
                fileInfo.MoveTo(destPath);
            }
            catch (Exception ex)
            {
                LogWriter.Write($"FileManipulator.RenameFile() - Can not rename (move) " +
                    $"'{filepath}' to '{destPath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private FileInfo GetFileInfo(string filepath)
        {
            FileInfo fileInfo;
            if (File.Exists(filepath))
                fileInfo = new FileInfo(filepath);
            else
                throw new IOException($"FileManipulator.GetFileInfo(): Cannot create a " +
                    $"FileInfo object from '{filepath}'. It does not exist.");
            return fileInfo;
        }

        private void RenameFileAsTemp(FileInfo currentFile)
        {
            var tempPath = currentFile.FullName.Replace(currentFile.Name, @"_temp");
            currentFile.MoveTo(tempPath);
            _systemFile.Filepath = tempPath;
        }

        // Refactoring TO-DOs:
        // Naming consistency - Move/Rename
        // Consider reordering methods - public ones at the top (RenameFile(string songTitle))
            // GetFileInfo under GetDirectoryInfo
    }
}
