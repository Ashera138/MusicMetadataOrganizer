using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MusicMetadataUpdater_v2._0
{
    public static class FileSearcher
    {
        private static List<SystemFile> _files = new List<SystemFile>(); 

        public static List<SystemFile> ExtractFiles(string directory)
        {
            _files.Clear();
            ExtractFilesRecursively(directory);
            return _files;
        }

        public static string SelectDirectory()
        {
            var directory = string.Empty;
            var folderBrowser = new FolderBrowserDialog
                { SelectedPath = @"Z:\Music" };

            if (folderBrowser.ShowDialog() == DialogResult.OK)
                directory = folderBrowser.SelectedPath;
            return directory;
        }

        private static void ExtractFilesRecursively(string directory)
        {
            var filesInFolder = Directory.EnumerateFiles(directory, "", SearchOption.AllDirectories);

            foreach (var path in Directory.EnumerateFiles(directory))
            {
                if (IsSupportedMediaFile(path))
                    _files.Add(new SystemFile(path));
            }

            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                ExtractFilesRecursively(subdirectory);
            }
        }

        private static bool IsSupportedMediaFile(string path)
        {
            return supportedMediaExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }

        private static string[] supportedMediaExtensions =
        {
            ".AAC", ".AIFF", ".APE", ".ASF", ".AA", ".AAX", ".FLAC", ".MKA", ".M4A", ".MP3",
            ".MPC", ".OGG", ".RIFF", ".WV"
        };        
    }
}
