using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicMetadataUpdater_v2._0
{
    public static class FileManipulator
    { 
        public static DirectoryInfo GetDirectoryInfo(string directory)
        {
            DirectoryInfo directoryInfo;
            if (Directory.Exists(directory))
                directoryInfo = new DirectoryInfo(directory);
            else
                throw new IOException($"FileManipulator.GetDirectoryInfo(): Cannot create a " +
                    $"DirectoryInfo object from '{directory}'. It does not exist.");
            return directoryInfo;
        }

        public static FileInfo GetFileInfo(string filepath)
        {
            FileInfo fileInfo;
            if (File.Exists(filepath))
                fileInfo = new FileInfo(filepath);
            else
                throw new IOException($"FileManipulator.GetFileInfo(): Cannot create a " +
                    $"FileInfo object from '{filepath}'. It does not exist.");
            return fileInfo;
        }

        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public static void DeleteEmptyFolders(DirectoryInfo folder)
        {
            if (HasFiles(folder))
                return;
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

        private static bool HasFiles(DirectoryInfo folder)
        {
            var databaseFileExtension = ".db";
            List<FileInfo> files = folder.EnumerateFiles().Where(f => f.Extension != databaseFileExtension).ToList();
            return files.Count() > 0 ? true : false;
        }

        // Make this recursive to check lower subdirectories?
        private static bool FilesExistInSubdirectories(DirectoryInfo folder)
        {
            bool hasFiles = false;
            foreach (DirectoryInfo directory in folder.EnumerateDirectories())
            {
                if (HasFiles(directory))
                    hasFiles = true;
            }
            return hasFiles;
        }
    }
}
