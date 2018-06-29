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
        private MetadataFile GetMetadataFile(string filepath = FileTestSharedVariables.originalFilepath)
        {
            return new MetadataFile(filepath);
        }

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

        private GracenoteAPIResponse GetMatchingAPIResponse(MetadataFile file)
        {
            return new GracenoteAPIResponse()
            {
                Artist = file.Artist,
                Album = file.Album,
                Title = file.Title,
                Year = file.Year,
                Genres = file.Genres,
                TrackNo = file.TrackNo
            };
        }

        private GracenoteAPIResponse GetAPIResponseWithDummyValues()
        {
            return new GracenoteAPIResponse()
            {
                Artist = "Fake Artist",
                Album = "Fake Album",
                Title = "Fake Title",
                Year = 2018,
                Genres = "Fake Genres",
                TrackNo = 1
            };
        }

        private bool AreEqual(Dictionary<string, string> dict1, Dictionary<string, string> dict2)
        {
            bool equal = true;

            foreach (var pair in dict1)
            {
                if (dict2.TryGetValue(pair.Key, out string value))
                {
                    if (value != pair.Value)
                    {
                        equal = false;
                        break;
                    }
                }
                else
                {
                    equal = false;
                    break;
                }
            }
            return equal;
        }

        [TestMethod]
        public void Constructor_Test_PopulatesBasicFieldsGivenValidFilepath()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                var metadataFile = GetMetadataFile();
                var properties = ExtractProperties(metadataFile);

                bool hasEmptyValues = properties.Any(pair => string.IsNullOrEmpty(pair.Value));

                Assert.IsFalse(hasEmptyValues);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void Constructor_Test_ThrowsFileNotFoundExceptionGivenInvalidFilepath()
        {
            var invalidFilepath = @"This\is\not\a\real\filepath.mp3";

            Assert.ThrowsException<FileNotFoundException>(() => GetMetadataFile(invalidFilepath));
        }

        [TestMethod]
        public void UpdateMetadata_Test_SuccessfullyUpdatesMetadataFileGivenAnAPIResponseWithNewValues()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                var metadataFile = GetMetadataFile();
                var propertiesBeforeUpdate = ExtractProperties(metadataFile);
                var gracenoteResponse = GetAPIResponseWithDummyValues();

                metadataFile.UpdateMetadata(gracenoteResponse);

                var propertiesAfterUpdate = ExtractProperties(metadataFile);
                var areEqual = AreEqual(propertiesBeforeUpdate, propertiesAfterUpdate);
                Assert.IsFalse(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void UpdateMetadata_Test_MetadataFileDoesNotChangeGivenAnAPIResponseWithNoNewValues()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                var metadataFile = GetMetadataFile();
                var propertiesBeforeUpdate = ExtractProperties(metadataFile);
                var gracenoteResponse = GetMatchingAPIResponse(metadataFile);

                metadataFile.UpdateMetadata(gracenoteResponse);

                var propertiesAfterUpdate = ExtractProperties(metadataFile);
                var areEqual = AreEqual(propertiesBeforeUpdate, propertiesAfterUpdate);
                Assert.IsTrue(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_ReturnsTrueForSuccessfulSave()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                var metadataFile = GetMetadataFile();

                var saveResult = metadataFile.TrySave();

                Assert.IsTrue(saveResult);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void Equals_Test_ReturnsTrueGivenEqualMetadataFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.copyOfOriginalFilepath);

                var metadataFile1 = GetMetadataFile();
                var metadataFile2 = GetMetadataFile(FileTestSharedVariables.copyOfOriginalFilepath);

                Assert.IsTrue(metadataFile1.Equals(metadataFile2));
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void Equals_Test_ReturnsFalseGivenNonEqualMetadataFiles()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.copyOfOriginalFilepath);

                var metadataFile1 = GetMetadataFile();
                var metadataFile2 = GetMetadataFile(FileTestSharedVariables.copyOfOriginalFilepath);
                metadataFile2.Artist = "Something different";

                Assert.IsFalse(metadataFile1.Equals(metadataFile2));
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void IsUpdateNeeded_Test_ReturnsTrueWhenResponseHasDifferentValues()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
                var metadataFile = GetMetadataFile();
                var gracenoteResponse = GetAPIResponseWithDummyValues();
                metadataFile.Response = gracenoteResponse;

                var isUpdateNeeded = metadataFile.IsUpdateNeeded();

                Assert.IsTrue(isUpdateNeeded);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void IsUpdateNeeded_Test_ReturnsFalseWhenResponseHasSameValues()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir(FileTestSharedVariables.originalFilepath);
                var metadataFile = GetMetadataFile();
                var gracenoteResponse = GetMatchingAPIResponse(metadataFile);
                metadataFile.Response = gracenoteResponse;

                var isUpdateNeeded = metadataFile.IsUpdateNeeded();

                Assert.IsFalse(isUpdateNeeded);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        // Not currently being tested - might eventually get rid of it
        //public void PopulateUpdateList()
    }
}
