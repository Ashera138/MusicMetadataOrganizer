using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.UnitTests.Database
{
    [TestClass]
    public class SystemFileRepository_Tests
    {
        private MockSystemFileRepository _mockSystemFileRepo = new MockSystemFileRepository();
        private readonly MockSystemFile _mockFile = new MockSystemFile()
        {
            SystemFileId = 55,
            Filepath = "Test filepath",
            Name = "Test Name",
            Directory = "Test Directory",
            Extension = ".test extension",
            CreationTime = DateTime.Now,
            LastAccessTime = DateTime.Now,
            LengthInBytes = 2050,
        };

        private bool IsMismatchFound(List<MockSystemFile> unalteredFiles, List<MockSystemFile> alteredFiles)
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
        public async Task AddSystemFileAsync_Test_ReturnsTrueForValidAddition()
        {
            _mockSystemFileRepo.GenerateDataTable(20);

            bool success = await _mockSystemFileRepo.AddFileAsync(_mockFile);
            if (success == false)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task AddSystemFileAsync_Test_RepoDataTableContainsAddedFile()
        {
            _mockSystemFileRepo.GenerateDataTable(20);
            await _mockSystemFileRepo.AddFileAsync(_mockFile);

            var mockFilesInDataTable = _mockSystemFileRepo.DataTable.Rows.ToList<MockSystemFile>();

            var success = false;
            foreach (MockSystemFile mockFileInDataTable in mockFilesInDataTable)
            {
                if (mockFileInDataTable.Equals(_mockFile))
                    success = true;
            }

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task AddSystemFileAsync_Test_FailsWhenFileAlreadyExistsInRepo()
        {
            _mockSystemFileRepo.GenerateDataTable(20);
            await _mockSystemFileRepo.AddFileAsync(_mockFile);
            Assert.IsFalse(await _mockSystemFileRepo.AddFileAsync(_mockFile));
        }

        [TestMethod]
        public async Task GetSystemFilesAsync_Test_ReturnsFiles()
        {
            _mockSystemFileRepo.GenerateDataTable();
            List<MockSystemFile> files = await _mockSystemFileRepo.GetFilesAsync();

            Assert.IsTrue(files != null && files.Count > 0);
        }

        [TestMethod]
        public async Task GetSystemFileAsync_Test_ValidInputReturnsNotNullFile()
        {
            _mockSystemFileRepo.GenerateDataTable();

            string filepath = "Filepath1";
            MockSystemFile file = await _mockSystemFileRepo.GetFileAsync(filepath);

            Assert.IsNotNull(file);
        }

        [TestMethod]
        public async Task GetSystemFileAsync_Test_ReturnsNullWhenFileDoesNotExist()
        {
            _mockSystemFileRepo.GenerateDataTable();
            string filepath = "Invalid filepath";

            MockSystemFile file = await _mockSystemFileRepo.GetFileAsync(filepath);

            Assert.IsNull(file);
        }

        [TestMethod]
        public async Task UpdateSystemFileAsync_Test_WhenRecordExists()
        {
            _mockSystemFileRepo.GenerateDataTable(20);
            MockSystemFile mockFile = new MockSystemFile() { SystemFileId = 1, Name = "New Name Value" };
            var unalteredFiles = await _mockSystemFileRepo.GetFilesAsync();

            await _mockSystemFileRepo.UpdateFileAsync(mockFile);
            var alteredFiles = _mockSystemFileRepo.DataTable.ToList<MockSystemFile>();

            bool isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsTrue(isMismatchFound);
        }

        [TestMethod]
        public async Task UpdateSystemFileAsync_Test_MethodReturnsTrueAfterSuccessfulUpdate()
        {
            _mockSystemFileRepo.GenerateDataTable(3);
            MockSystemFile mockFile = new MockSystemFile() { SystemFileId = 1, Name = "New Name Value" };

            var updateSuccessful = await _mockSystemFileRepo.UpdateFileAsync(mockFile);

            Assert.IsTrue(updateSuccessful);
        }

        [TestMethod]
        public async Task UpdateSystemFileAsync_Test_WhenRecordDoesNotExist()
        {
            _mockSystemFileRepo.GenerateDataTable(3);
            MockSystemFile mockFile = new MockSystemFile() { SystemFileId = 5, Name = "New Name Value" };
            var unalteredFiles = await _mockSystemFileRepo.GetFilesAsync();

            await _mockSystemFileRepo.UpdateFileAsync(mockFile);
            var alteredFiles = _mockSystemFileRepo.DataTable.ToList<MockSystemFile>();
            var isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsFalse(isMismatchFound);
        }

        [TestMethod]
        public async Task UpdateSystemFileAsync_Test_ReturnsFalseWhenRecordDoesNotExist()
        {
            _mockSystemFileRepo.GenerateDataTable(3);
            MockSystemFile mockFile = new MockSystemFile() { SystemFileId = 5, Name = "New Name Value" };

            var updateSuccessful = await _mockSystemFileRepo.UpdateFileAsync(mockFile);

            Assert.IsFalse(updateSuccessful);
        }

        [TestMethod]
        public async Task DeleteSystemFileAsync_Test_RepoDoesNotContainFileAfterDeletion()
        {
            _mockSystemFileRepo.GenerateDataTable(50);
            await _mockSystemFileRepo.AddFileAsync(_mockFile);
            await _mockSystemFileRepo.DeleteFileAsync(_mockFile.Filepath);

            Assert.IsFalse(await _mockSystemFileRepo.ContainsAsync(_mockFile));
        }

        [TestMethod]
        public async Task ContainsAsync_Test_ContainsFileAfterAddSystemFileAsyncMethod()
        {
            _mockSystemFileRepo.GenerateDataTable(20);
            await _mockSystemFileRepo.AddFileAsync(_mockFile);

            if (!await _mockSystemFileRepo.ContainsAsync(_mockFile))
                Assert.Fail();
        }
    }
}
