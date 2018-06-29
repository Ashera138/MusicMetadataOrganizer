using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class FileSearcher_Tests
    {
        [TestMethod]
        public void ExtractFiles_Test_ReturnsEmptyListGivenADirectoryWithNoParsableFiles()
        {
            try
            {
                FileTestSharedVariables.CreateTestDirectory();

                var list = FileSearcher.ExtractFiles(FileTestSharedVariables.testDirectory);

                Assert.IsTrue(list.Count == 0);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void ExtractFiles_Test_ReturnsPopulatedListGivenADirectoryWithParsableFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();

                var list = FileSearcher.ExtractFiles(FileTestSharedVariables.testDirectory);

                Assert.IsTrue(list.Count > 0);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void ExtractFiles_Test_ThrowsDirectoryNotFoundExceptionGivenInvalidDirectory()
        {
            var invalidDirectory = "This is not a valid directory";

            Assert.ThrowsException<DirectoryNotFoundException>(() => FileSearcher.ExtractFiles(invalidDirectory));
        }
    }
}
