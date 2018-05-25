using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;
using MusicMetadataUI;

namespace MusicMetadataUpdater.IntegrationTests.Database
{
    [TestClass]
    public class MetadataFileRepository_Tests
    {
        private readonly string _filepath = @"C:\Users\Ashie\Desktop\The Adventure.mp3";
        private MetadataFileRepository _metadataFileRepo = new MetadataFileRepository();
        private readonly MetadataFile _metadataFile;

        public MetadataFileRepository_Tests()
        {
            _metadataFile = new MetadataFile(_filepath);
        }

        private bool IsMismatchFound(List<MetadataFile> unalteredFiles, List<MetadataFile> alteredFiles)
        {
            bool isMismatchFound = false;

            for (int i = 0; i < unalteredFiles.Count; i++)
            {
                if (!unalteredFiles[i].Equals(alteredFiles[i]))
                    isMismatchFound = true;
            }

            return isMismatchFound;
        }

        // Fails
        [TestMethod]
        public async Task AddMetadataFileAsync_Test_ReturnsTrueForValidAddition()
        {
            bool success = await _metadataFileRepo.AddFileAsync(_metadataFile);
            if (success == false)
            {
                Assert.Fail();
            }
        }

        // Fails
        [TestMethod]
        public async Task AddMetadataFileAsync_Test_RepoDataGridContainsAddedFile()
        {
            await _metadataFileRepo.AddFileAsync(_metadataFile);

            var filesInDbTable = await _metadataFileRepo.GetFilesAsync();

            var success = false;
            foreach (MetadataFile fileInDbTable in filesInDbTable)
            {
                if (fileInDbTable.Equals(_metadataFile))
                    success = true;
            }

            Assert.IsTrue(success);
        }

        // Passes
        [TestMethod]
        public async Task AddMetadataFileAsync_Test_FailsWhenFileAlreadyExistsInRepo()
        {
            await _metadataFileRepo.AddFileAsync(_metadataFile);
            Assert.IsFalse(await _metadataFileRepo.AddFileAsync(_metadataFile));
        }

        // Fails
        [TestMethod]
        public async Task GetMetadataFilesAsync_Test_ReturnsFiles()
        {
            List<MetadataFile> files = await _metadataFileRepo.GetFilesAsync();

            Assert.IsTrue(files != null && files.Count > 0);
        }

        // Fails
        [TestMethod]
        public async Task GetMetadataFileAsync_Test_ValidInputReturnsNotNullFile()
        {
            MetadataFile file = await _metadataFileRepo.GetFileAsync(_filepath);

            Assert.IsNotNull(file);
        }

        // Fails
        [TestMethod]
        public async Task GetMetadataFileAsync_Test_ReturnsNullWhenFileDoesNotExist()
        {
            string filepath = "Invalid filepath";

            MetadataFile file = await _metadataFileRepo.GetFileAsync(filepath);

            Assert.IsNull(file);
        }

        /*
        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_WhenRecordExists()
        {
            MetadataFile file = new MetadataFile() { MetadataFileId = 1, Artist = "TestArtistValue" };
            var unalteredFiles = await _metadataFileRepo.GetFilesAsync();

            await _metadataFileRepo.UpdateFileAsync(file);
            var alteredFiles = _metadataFileRepo.DataTable.ToList<MetadataFile>();

            bool isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsTrue(isMismatchFound);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_MethodReturnsTrueAfterSuccessfulUpdate()
        {
            MetadataFile file = new MetadataFile() { MetadataFileId = 1, Artist = "TestArtistValue" };

            var updateSuccessful = await _metadataFileRepo.UpdateFileAsync(file);

            Assert.IsTrue(updateSuccessful);
        }

        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_WhenRecordDoesNotExist()
        {
            MetadataFile file = new MetadataFile() { MetadataFileId = 5, Filepath = "Filepath5" };
            var unalteredFiles = await _metadataFileRepo.GetFilesAsync();

            await _metadataFileRepo.UpdateFileAsync(file);
            var alteredFiles = _metadataFileRepo.DataTable.ToList<MetadataFile>();
            var isMismatchFound = IsMismatchFound(unalteredFiles, alteredFiles);

            Assert.IsFalse(isMismatchFound);
        }
 
        [TestMethod]
        public async Task UpdateMetadataFileAsync_Test_ReturnsFalseWhenRecordDoesNotExist()
        {
            MetadataFile file = new MetadataFile() { MetadataFileId = 5, Filepath = "Filepath5" };

            var updateSuccessful = await _metadataFileRepo.UpdateFileAsync(file);

            Assert.IsFalse(updateSuccessful);
        }
        */

        // Fails
        [TestMethod]
        public async Task DeleteMetadataFileAsync_Test_RepoDoesNotContainFileAfterDeletion()
        {
            await _metadataFileRepo.AddFileAsync(_metadataFile);
            await _metadataFileRepo.DeleteFileAsync(_metadataFile.Filepath);

            Assert.IsFalse(await _metadataFileRepo.ContainsAsync(_metadataFile));
        }

        // Fails
        [TestMethod]
        public async Task ContainsAsync_Test_ContainsFileAfterAddMetadataFileAsyncMethod()
        {
            await _metadataFileRepo.AddFileAsync(_metadataFile);

            if (!await _metadataFileRepo.ContainsAsync(_metadataFile))
                Assert.Fail();
        }
    }
}
