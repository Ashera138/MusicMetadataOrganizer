using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class FileManipulator_Tests
    {
        [TestMethod]
        public void DeleteEmptyFolders_Test_EmptyFoldersAreDeleted()
        {
            try
            {
                string testDirectory = FileTestSharedVariables.baseTestDirectory;
                Directory.CreateDirectory(testDirectory);

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

                bool deletionSuccessful = !Directory.Exists(testDirectory);
                Assert.IsTrue(deletionSuccessful);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_EmptyParentFolderGetsDeletedWhenNoFilesExistInSingleSubdirector()
        {
            try
            {
                string testDirectory = Path.Combine(FileTestSharedVariables.baseTestDirectory, @"Empty Subdirectory\");
                Directory.CreateDirectory(testDirectory);

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

                bool deletionSuccessful = !Directory.Exists(FileTestSharedVariables.baseTestDirectory);
                Assert.IsTrue(deletionSuccessful);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_EmptyParentFolderGetsDeletedWhenNoFilesExistInMultipleSubdirectories()
        {
            try
            {
                string middleTestDirectory = Path.Combine(FileTestSharedVariables.baseTestDirectory, @"First Empty Dir\");
                string bottomTestDirectory = Path.Combine(middleTestDirectory, @"Second Empty Dir\");
                Directory.CreateDirectory(bottomTestDirectory);

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(middleTestDirectory));

                bool deletionSuccessful = !Directory.Exists(FileTestSharedVariables.baseTestDirectory);
                Assert.IsTrue(deletionSuccessful);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenFolderHasFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                string testDirectory = Path.Combine(
                    FileTestSharedVariables.baseTestDirectory, @"Angels and Airwaves\We Don't Need to Whisper\");

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

                bool testDirectoryExists = Directory.Exists(testDirectory);
                Assert.IsTrue(testDirectoryExists);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenParentFolderHasFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                string testDirectory = Path.Combine(
                    FileTestSharedVariables.baseTestDirectory, @"Angels and Airwaves\We Don't Need to Whisper\");
                Directory.CreateDirectory(Path.Combine(testDirectory, @"Empty Bottom Folder\"));

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

                bool testDirectoryExists = Directory.Exists(testDirectory);
                Assert.IsTrue(testDirectoryExists);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenSubFolderHasFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                string testDirectory = Path.Combine(FileTestSharedVariables.baseTestDirectory, @"Angels and Airwaves\");
                string testSubdirectory = Path.Combine(testDirectory, @"We Don't Need to Whisper\");

                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

                bool testDirectoryExists = Directory.Exists(testSubdirectory);
                Assert.IsTrue(testDirectoryExists);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_InaccessibleDirectoryInput()
        {
            string inaccessibleDirectory = @"C:\PerfLogs\";

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(inaccessibleDirectory));

            bool directoryExists = Directory.Exists(inaccessibleDirectory);
            Assert.IsTrue(directoryExists);
        }
    }
}
