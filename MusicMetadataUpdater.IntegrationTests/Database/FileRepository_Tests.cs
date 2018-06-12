using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;
using MusicMetadataUI.Database;
using System.Linq;

namespace MusicMetadataUpdater.IntegrationTests.Database
{
    [TestClass]
    public class FileRepository_Tests
    {
        private FileRepository _fileRepo = new FileRepository();
        private SystemFile _systemFile;

        public FileRepository_Tests()
        {
            _systemFile = new SystemFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
        }

        private async Task DeleteTestFileFromDatabase(string filepath)
        {
            await _fileRepo.DeleteFileAsync(filepath);
        }

        [TestMethod]
        public async Task AddFileAsync_Test_ReturnsTrueForValidInput()
        {
            try
            {
                bool success = await _fileRepo.AddFileAsync(_systemFile);

                Assert.IsTrue(success);
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task AddFileAsync_Test_RepoContainsAddedFileGivenValidInput()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                var filesInDatabase = await _fileRepo.GetFilesAsync();

                bool matchExsits = filesInDatabase.Any(f => f.Equals(_systemFile));
                Assert.IsTrue(matchExsits);
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task AddFileAsync_Test_FailsWhenFileAlreadyExistsInRepo()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                Assert.IsFalse(await _fileRepo.AddFileAsync(_systemFile));
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task GetFilesAsync_Test_ReturnsPopulatedListWhenRecordsExist()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                List<SystemFile> files = await _fileRepo.GetFilesAsync();

                Assert.IsTrue(files.Count > 0);
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task GetFilesAsync_Test_ReturnsEmptyListWhenNoRecordsExist()
        {
            List<SystemFile> files = await _fileRepo.GetFilesAsync();

            Assert.IsTrue(files.Count == 0);
        }

        [TestMethod]
        public async Task GetFileAsync_Test_ReturnsNotNullFileGivenValidInpu()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                SystemFile file = await _fileRepo.GetFileAsync(_systemFile.Filepath);

                Assert.IsNotNull(file);
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task GetFileAsync_Test_ReturnsNullWhenRecordDoesNotExist()
        {
            string filepath = "Invalid filepath";

            SystemFile file = await _fileRepo.GetFileAsync(filepath);

            Assert.IsNull(file);
        }

        [TestMethod]
        public async Task UpdateFileAsync_Test_ReturnsTrueWhenFileExistsInDatabase()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                var updateSuccessful = await _fileRepo.UpdateFileAsync(_systemFile);

                Assert.IsTrue(updateSuccessful);
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task UpdateFileAsync_Test_ReturnsFalseWhenFileDoesNotExistsInDatabase()
        {
            var updateSuccessful = await _fileRepo.UpdateFileAsync(_systemFile);

            Assert.IsFalse(updateSuccessful);
        }

        [TestMethod]
        public async Task DeleteFileAsync_Test_RepoDoesNotContainFileAfterDeletion()
        {
            await _fileRepo.AddFileAsync(_systemFile);

            await _fileRepo.DeleteFileAsync(_systemFile.Filepath);

            Assert.IsFalse(await _fileRepo.ContainsAsync(_systemFile));
        }

        [TestMethod]
        public async Task DeleteFileAsync_Test_NoExceptionsThrownWhenDeletingNonExistentRecord()
        {
            await _fileRepo.DeleteFileAsync(_systemFile.Filepath);
        }

        [TestMethod]
        public async Task ContainsAsync_Test_ReturnsTrueAfterAddingFile()
        {
            try
            {
                await _fileRepo.AddFileAsync(_systemFile);

                Assert.IsTrue(await _fileRepo.ContainsAsync(_systemFile));
            }
            finally
            {
                await DeleteTestFileFromDatabase(_systemFile.Filepath);
            }
        }

        [TestMethod]
        public async Task ContainsAsync_Test_ReturnsFalseWhenRecordDoesNotExist()
        {
            Assert.IsFalse(await _fileRepo.ContainsAsync(_systemFile));
        }
    }
}
