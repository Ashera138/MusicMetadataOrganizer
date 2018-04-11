using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MusicMetadataUpdater_v2._0
{
    // TO:DO - Refactor all of this class
    public static class FileSearcher
    {
        [STAThread]
        public static List<SystemFile> ExtractFiles()
        {
            var directory = SelectDirectory();
            return ExtractFiles(directory);
        }

        private static string SelectDirectory()
        {
            var directory = string.Empty;

            var folderBrowser = new FolderBrowserDialog
            {
                SelectedPath = @"Z:\Music"
            };

            if (folderBrowser.ShowDialog() == DialogResult.OK)
                directory = folderBrowser.SelectedPath;
            else
            {
                Environment.Exit(1);
                Application.Exit();
            }

            return directory;
        }

        // Fix this nightmare
        // Make unit tests
        private static List<SystemFile> ExtractFiles(string directory)
        {
            var files = new List<SystemFile>();
            var filesInFolder = Directory.EnumerateFiles(directory, "", SearchOption.AllDirectories);

            foreach (var path in filesInFolder)
            {
                if (IsMediaFile(path))
                    files.Add(new SystemFile(path));
            }

            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                ExtractFiles(subdirectory);
            }
            return files;
        }

        /*
        private static void ExtractFiles(string directory)
        {
            var filesInFolder = Directory.EnumerateFiles(directory, "", SearchOption.AllDirectories);

            foreach (var path in Directory.EnumerateFiles(directory))
            {
                if (IsMediaFile(path))
                    files.Add(MasterFile.GetMasterFileFromFilepath(path));
            }

            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                ExtractFiles(subdirectory);
            }
        }
        */

        [Obsolete("Not being used anywhere in v1.0. Double check for deletion or possibly use instead.")]
        internal static IEnumerable<SystemFile> ExtractFilesFromFolder(string directory)
        {
            foreach (var path in Directory.EnumerateFiles(directory))
            {
                if (IsMediaFile(path))
                    yield return new SystemFile(path);
            }
        }

        private static bool IsMediaFile(string path)
        {
            return mediaExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }

        private static string[] mediaExtensions =
        {
            ".AAC", ".AIFF", ".APE", ".ASF", ".AA", ".AAX", ".FLAC", ".MKA", ".M4A", ".MP3",
            ".MPC", ".OGG", ".RIFF", ".WV"
        };        
    }
}
