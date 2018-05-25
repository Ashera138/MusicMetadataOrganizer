using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class MetadataFile_Tests
    {
        private Dictionary<string, string> ExtractProperties(MetadataFile metadataFile)
        {
            return new Dictionary<string, string>
            {
                { "Filepath", metadataFile.Filepath },
                { "BitRate", metadataFile.BitRate.ToString() },
                { "MediaType", metadataFile.MediaType },
                { "Artist", metadataFile.Artist },
                { "Album", metadataFile.Album },
                { "Title", metadataFile.Title },
                { "DurationInTicks", metadataFile.DurationInTicks.ToString() }
            };
        }

        [TestMethod]
        public void Test_PopulatesBasicFieldsUponInitialization_GivenValidFilepath()
        {
            FileTestSharedVariables.CopyFileToTestDir();

            var metadataFile = new MetadataFile(FileTestSharedVariables.originalFilepath);
            var properties = ExtractProperties(metadataFile);
            bool hasEmptyValues = properties.Any(pair => string.IsNullOrEmpty(pair.Value));

            FileTestSharedVariables.DeleteTestDirectory();
            if (hasEmptyValues)
                Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Test_IOExceptionUponInitialization_GivenInvalidFilepath()
        {
            var invalidFilepath = @"This\is\not\a\real\filepath.mp3";
            var metadataFile = new MetadataFile(invalidFilepath);
        }

        [TestMethod]
        public void UpdateMetadataWithAPIResult_Test_Condition()
        {
            // Write test and determine condition... update name ^
            Assert.Fail("Not yet implemented.");
        }

        [TestMethod]
        public void TrySave_Test_Condition()
        {
            // Write test and determine condition... update name ^
            Assert.Fail("Not yet implemented.");
        }

        [TestMethod]
        public void Equals_Test_ReturnsTrueWhenEqual()
        {
            FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
            FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.copyOfOriginalFilepath);

            var metadataFile1 = new MetadataFile(FileTestSharedVariables.originalFilepath);
            var metadataFile2 = new MetadataFile(FileTestSharedVariables.copyOfOriginalFilepath);

            FileTestSharedVariables.DeleteTestDirectory();
            if (!metadataFile1.Equals(metadataFile2))
                Assert.Fail("Files are not equal.");
        }

        [TestMethod]
        public void Equals_Test_ReturnsFalseWhenNotEqual()
        {
            FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
            FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.copyOfOriginalFilepath);

            var metadataFile1 = new MetadataFile(FileTestSharedVariables.originalFilepath);
            var metadataFile2 = new MetadataFile(FileTestSharedVariables.copyOfOriginalFilepath);
            metadataFile2.Artist = "Something different";

            FileTestSharedVariables.DeleteTestDirectory();
            if (metadataFile1.Equals(metadataFile2))
                Assert.Fail("Files are not equal.");
        }
    }
}
