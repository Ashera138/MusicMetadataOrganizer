using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;
using System.Linq;
using MusicMetadataUpdater.UnitTests.MockFile;

namespace MusicMetadataUpdater.UnitTests.Database
{
    // Refactoring: Clean up existing methods
    // Add more test cases
    // Arrange blank lines within methods based on AAA grouping

    [TestClass]
    public class MetadataFileRepository_Tests
    {
        private MockMetadataFileRepository _mockMetadataFileRepo = new MockMetadataFileRepository();
        private readonly MockMetadataFile _mockFile = new MockMetadataFile()
        {
            MetadataFileId = 55,
            Filepath = "Test filepath",
            BitRate = 0,
            MediaType = "Audio",
            Artist = "Test artist",
            Album = "Test album",
            Title = "Test title",
            Genres = "Test genres",
            Lyrics = "Test lyrics",
            TrackNo = null,
            Year = null,
            Rating = null,
            DurationInTicks = 0
        };

        private bool IsMismatchFound(List<MockMetadataFile> unalteredFiles, List<MockMetadataFile> alteredFiles)
        {
            bool isMismatchFound = false;

            for (int i = 0; i < unalteredFiles.Count; i++)
            {
                if (!unalteredFiles[i].Equals(alteredFiles[i]))
                    isMismatchFound = true;
            }

            return isMismatchFound;
        }

        [TestMethod]
        public async Task AddMetadataFileAsync_Test_ReturnsTrueForValidAddition()
        {
            _mockMetadataFileRepo.GenerateDataTable(20);

            bool success = await _mockMetadataFileRepo.AddFileAsync(_mockFile);
            if (success == false)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task AddMetadataFileAsync_Test_RepoDataTableContainsAddedFile()
        {
            _mockMetadataFileRepo.GenerateDataTable(20);
            await _mockMetadataFileRepo.AddFileAsync(_mockFile);

            var mockFilesInDataTable = _mockMetadataFileRepo.DataTable.Rows.ToList<MockMetadataFile>();

            var success = false;
            foreach (MockMetadataFile mockFileInDataTable in mockFilesInDataTable)
            {
                if (mockFileInDataTable.Equals(_mockFile))
                    success = true;
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task AddMetadataFileAsync_Test_FailsWhenFileAlreadyExistsInRepo()
        {
            _mockMetadataFileRepo.GenerateDataTable(20);
            await _mockMetadataFileRepo.AddFileAsync(_mockFile);
            Assert.IsFalse(await _mockMetadataFileRepo.AddFileAsync(_mockFile));
        }

        [TestMethod]
        public async Task GetMetadataFilesAsync_Test_ReturnsFiles()
        {
            _mockMetadataFileRepo.GenerateDataTable();
            List<MockMetadataFile> files = await _mockMetadataFileRepo.GetFilesAsync();

            Assert.IsTrue(files != null && files.Count > 0);
        }

        [TestMethod]
        public async Task GetMetadataFileAsync_Test_ValidInputReturnsNotNullFile()
        {
            _mockMetadataFileRepo.GenerateDataTable();

            string filepath = "Filepath1";
            MockMetadataFile file = await _mockMetadataFileRepo.GetFileAsync(filepath);

            Assert.IsNotNull(file);
        }

        [TestMethod]
        public async Task GetMetadataFileAsync_Test_ReturnsNullWhenFileDoesNotExist()
        {
            _mockMetadataFileRepo.GenerateDataTable();
            string filepath = "Invalid filepath";

            MockMetadataFile file = await _mockMetadataFileRepo.GetFileAsync(filepath);

            Assert.IsNull(file);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_WhenRecordExists()
        {
            _mockMetadataFileRepo.GenerateDataTable(20);
            MockMetadataFile mockFile = new MockMetadataFile() { MetadataFileId = 1, Artist = "TestArtistValue" };
            var unalteredFiles = await _mockMetadataFileRepo.GetFilesAsync();

            await _mockMetadataFileRepo.UpdateFileAsync(mockFile);
            var alteredFiles = _mockMetadataFileRepo.DataTable.ToList<MockMetadataFile>();

            bool isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsTrue(isMismatchFound);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_MethodReturnsTrueAfterSuccessfulUpdate()
        {
            _mockMetadataFileRepo.GenerateDataTable(3);
            MockMetadataFile mockFile = new MockMetadataFile() { MetadataFileId = 1, Artist = "TestArtistValue" };

            var updateSuccessful = await _mockMetadataFileRepo.UpdateFileAsync(mockFile);

            Assert.IsTrue(updateSuccessful);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_WhenRecordDoesNotExist()
        {
            _mockMetadataFileRepo.GenerateDataTable(3);
            MockMetadataFile mockFile = new MockMetadataFile() { MetadataFileId = 5, Filepath = "Filepath5" };
            var unalteredFiles = await _mockMetadataFileRepo.GetFilesAsync();

            await _mockMetadataFileRepo.UpdateFileAsync(mockFile);
            var alteredFiles = _mockMetadataFileRepo.DataTable.ToList<MockMetadataFile>();
            var isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsFalse(isMismatchFound);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_ReturnsFalseWhenRecordDoesNotExist()
        {
            _mockMetadataFileRepo.GenerateDataTable(3);
            MockMetadataFile mockFile = new MockMetadataFile() { MetadataFileId = 5, Filepath = "Filepath5" };

            var updateSuccessful = await _mockMetadataFileRepo.UpdateFileAsync(mockFile);

            Assert.IsFalse(updateSuccessful);
        }

        [TestMethod]
        public async Task DeleteMetadataFileAsync_Test_RepoDoesNotContainFileAfterDeletion()
        {
            _mockMetadataFileRepo.GenerateDataTable(50);
            await _mockMetadataFileRepo.AddFileAsync(_mockFile);
            await _mockMetadataFileRepo.DeleteFileAsync(_mockFile.Filepath);

            Assert.IsFalse(await _mockMetadataFileRepo.ContainsAsync(_mockFile));
        }

        [TestMethod]
        public async Task ContainsAsync_Test_ContainsFileAfterAddMetadataFileAsyncMethod()
        {
            _mockMetadataFileRepo.GenerateDataTable(20);
            await _mockMetadataFileRepo.AddFileAsync(_mockFile);

            if (!await _mockMetadataFileRepo.ContainsAsync(_mockFile))
                Assert.Fail();
        }
    }
}
