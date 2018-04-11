using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MusicMetadataUpdater_v2._0
{
    internal static class FileManipulator
    {
        internal static DirectoryInfo GetDirectoryInfo(string directory)
        {
            DirectoryInfo directoryInfo;
            if (Directory.Exists(directory))
                directoryInfo = new DirectoryInfo(directory);
            else
                throw new IOException($"FileManipulator.GetDirectoryInfo(): Cannot create a " +
                    $"DirectoryInfo object from '{directory}'. It does not exist.");
            return directoryInfo;
        }

        internal static FileInfo GetFileInfo(string filepath)
        {
            FileInfo fileInfo;
            if (File.Exists(filepath))
                fileInfo = new FileInfo(filepath);
            else
                throw new IOException($"FileManipulator.GetFileInfo(): Cannot create a " +
                    $"FileInfo object from '{filepath}'. It does not exist.");
            return fileInfo;
        }

        internal static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        internal static void DeleteEmptyFolders(DirectoryInfo folder)
        {
            try
            {
                if (HasAccessToFolder(folder.FullName))
                {
                    DeleteEmptySubdirectories(folder);
                    if (!HasFiles(folder) && !FilesExistInSubdirectories(folder))
                    {
                        Directory.Delete(folder.FullName);
                        if (!HasFiles(folder.Parent) && !FilesExistInSubdirectories(folder.Parent))
                            folder.Parent.Delete();
                    }
                }
            }
            catch (IOException ex)
            {
                LogWriter.Write($"FileManipulator.DeleteEmptyFoldres(): Cannot delete {folder}. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private static bool HasAccessToFolder(string folderPath)
        {
            bool hasAccess = false;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            DirectorySecurity directorysecurity = Directory.GetAccessControl(folderPath);
            var rules = directorysecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
            foreach (FileSystemAccessRule rule in rules)
            {
                if (identity.Groups.Contains(rule.IdentityReference))
                {
                    if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                    {
                        if (rule.AccessControlType == AccessControlType.Allow)
                            hasAccess = true;
                    }
                }
            }
            return hasAccess;
        }

        private static void DeleteEmptySubdirectories(DirectoryInfo folder)
        {
            foreach (var subdirectoryPath in GetEmptyDirectories(folder.FullName))
            {
                var subdirectory = GetDirectoryInfo(subdirectoryPath);
                if (!HasFiles(subdirectory))
                {
                    DeleteEmptySubdirectories(subdirectory);
                    Directory.Delete(subdirectoryPath);
                }
            }
        }

        private static bool HasFiles(DirectoryInfo folder)
        {
            var databaseFileExtension = ".db";
            List<FileInfo> files = folder.EnumerateFiles().Where(f => f.Extension != databaseFileExtension).ToList();
            return files.Count() > 0 ? true : false;
        }

        private static bool FilesExistInSubdirectories(DirectoryInfo folder)
        {
            bool hasFiles = false;
            foreach (var directory in GetEmptyDirectories(folder.FullName))
            {
                if (HasFiles(GetDirectoryInfo(directory)))
                    hasFiles = true;
            }
            return hasFiles;
        }

        private static List<string> GetEmptyDirectories(string directory, string searchPattern = "*",
                                                  SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(directory, searchPattern).ToList();

            var directories = new List<string>(GetEmptyDirectories(directory, searchPattern));

            for (int i = 0; i < directories.Count(); i++)
            {
                directories.AddRange(GetEmptyDirectories(directories[i], searchPattern));
            }

            return directories;
        }

        private static List<string> GetEmptyDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }
    }
}
