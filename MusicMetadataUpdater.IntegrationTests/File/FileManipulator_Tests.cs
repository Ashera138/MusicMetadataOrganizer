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
            var testDirectory = @"C:\_TempForTesting\";
            Directory.CreateDirectory(testDirectory);

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

            bool success = !Directory.Exists(testDirectory);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("Original empty directory exists; it did not get deleted.");
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_EmptyParentFolderGetsDeleted_WhenNoFilesExistInSingleSubdirector()
        {
            var testDirectory = @"C:\_TempForTesting\Empty Subdirectory\";
            Directory.CreateDirectory(testDirectory);

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(@"C:\_TempForTesting\Empty Subdirectory\"));

            bool success = !Directory.Exists(@"C:\_TempForTesting\");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_EmptyParentFolderGetsDeleted_WhenNoFilesExistInMultipleSubdirectories()
        {
            var testDirectory = @"C:\_TempForTesting\First Empty Dir\Second Empty Dir\";
            Directory.CreateDirectory(testDirectory);

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(@"C:\_TempForTesting\First Empty Dir\"));

            bool success = !Directory.Exists(@"C:\_TempForTesting\");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenFolderHasFiles()
        {
            var testDirectory = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\";
            FileTestSharedVariables.CopyFileToTestDir();

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

            bool success = Directory.Exists(testDirectory);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The test directory was deleted even though it had a file.");
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenParentFolderHasFiles()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            var testDirectory = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\";
            Directory.CreateDirectory(Path.Combine(testDirectory, @"Empty Bottom Folder\"));

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(testDirectory));

            bool success = Directory.Exists(testDirectory);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The test directory was deleted even though its parent directory had a file.");
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_NoDeletionsWhenSubFolderHasFiles()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            var testDirectory = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\";

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(@"C:\_TempForTesting\Angels and Airwaves\"));

            bool success = Directory.Exists(testDirectory);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The test directory was deleted even though its subdirectory had a file.");
        }

        [TestMethod]
        public void DeleteEmptyFolders_Test_InaccessibleDirectoryInput()
        {
            var inaccessibleDirectory = @"C:\PerfLogs\";

            FileManipulator.DeleteEmptyFolders(new DirectoryInfo(inaccessibleDirectory));

            if (!Directory.Exists(inaccessibleDirectory))
                Assert.Fail("");
        }
    }
}
