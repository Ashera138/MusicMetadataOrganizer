using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class SystemFile_Tests
    {
        private SystemFile _systemFile;
        readonly string artist = "Angels and Airwaves";
        readonly string album = "We Don't Need to Whisper";
        readonly string song = "The Adventure.mp3";
        readonly string exceedsMaxCharString = "THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST " +
                                      "MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS " +
                                      "THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST " +
                                      "MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST MAX PATH CHAR CONDITIONS";

        private SystemFile GetSystemFile(string filepath = 
            @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\The Adventure.mp3")
        {
            return new SystemFile(filepath);
        }

        [TestMethod]
        public void TrySave_Test_FileIsMovedFromOriginalLocationAfterValidArtistRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Artist = "Test Artist";

                _systemFile.TrySave();

                bool fileExistInOriginalLocation = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
                Assert.IsFalse(fileExistInOriginalLocation);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_FileExistsInNewLocationAfterValidArtistRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Artist = "Test Artist";

                _systemFile.TrySave();

                bool fileExistsInNewLocation = System.IO.File.Exists($@"C:\_TempForTesting\Test Artist\{album}\{song}");
                Assert.IsTrue(fileExistsInNewLocation);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_MovesSuccesfullyWhenNewArtistNameOnlyDiffersByCase()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Artist = "aNgElS aNd AiRwAvEs";
                var originalFilepath = _systemFile.Filepath;

                _systemFile.TrySave();

                var newFilePath = _systemFile.Filepath;
                bool areEqual = (originalFilepath == newFilePath);
                Assert.IsFalse(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_DoesNotCreateDestinationDirectoryGivenArtistThatExceedsMaxPathLimit()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Artist = exceedsMaxCharString;

                _systemFile.TrySave();

                var directoryExists = Directory.Exists($@"C:\_TempForTesting\{exceedsMaxCharString}\{album}\{song}");
                Assert.IsFalse(directoryExists);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_FileIsMovedFromOriginalLocationAfterValidAlbumRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Album = "Test Album";

                _systemFile.TrySave();

                var fileExistsInOriginalLocation = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
                Assert.IsFalse(fileExistsInOriginalLocation);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_FileExistsInNewLocationAfterValidAlbumRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Album = "Test Album";

                _systemFile.TrySave();

                var fileExistsInDestinationLocation = System.IO.File.Exists(
                    $@"C:\_TempForTesting\{artist}\Test Album\{song}");
                Assert.IsTrue(fileExistsInDestinationLocation);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();           
            }
        }

        [TestMethod]
        public void TrySave_Test_MovesSuccesfullyWhenNewAlbumNameOnlyDiffersByCase()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Album = "wE dOn'T nEeD tO wHiSpEr";
                var originalFilepath = _systemFile.Filepath;

                _systemFile.TrySave();

                var newFilePath = _systemFile.Filepath;
                var areEqual = (originalFilepath == newFilePath);
                Assert.IsFalse(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_DoesNotCreateDestinationDirectoryGivenAlbumThatExceedsMaxPathLimit()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Album = exceedsMaxCharString;

                _systemFile.TrySave();

                var directoryExists = Directory.Exists($@"C:\_TempForTesting\{artist}\{exceedsMaxCharString}\{song}");
                Assert.IsFalse(directoryExists);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_FileDoesNotHaveOldNameAfterValidFileRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Title = "Test name";

                _systemFile.TrySave();

                var fileExistsWithOriginalName = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
                Assert.IsFalse(fileExistsWithOriginalName);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_FileHasNewNameAfterValidFileRename()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Title = "Test name";

                _systemFile.TrySave();

                var fileExistsWithNewName = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\Test name.mp3");
                Assert.IsTrue(fileExistsWithNewName);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_RenamesSuccesfullyWhenNewFileNameOnlyDiffersByCase()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Title = "tHe AdVeNtUrE";
                var originalFilepath = _systemFile.Filepath;

                _systemFile.TrySave();

                var newFilePath = _systemFile.Filepath;

                var areEqual = (originalFilepath == newFilePath);
                Assert.IsFalse(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_DoesNotCreateDestinationDirectoryGivenFileNameThatExceedsMaxPathLimit()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Title = exceedsMaxCharString;

                _systemFile.TrySave();

                var directoryExists = Directory.Exists($@"C:\_TempForTesting\{album}\{exceedsMaxCharString}.mp3");
                Assert.IsFalse(directoryExists);
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
                _systemFile = GetSystemFile();

                var saveResult = _systemFile.TrySave();

                Assert.IsTrue(saveResult);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_ReturnsFalseForFailedSave()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile.Artist = exceedsMaxCharString;

                var saveResult = _systemFile.TrySave();

                Assert.IsFalse(saveResult);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void TrySave_Test_ThrowsArgumentNullExceptionWhenMetadataFieldValueIsNull()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                _systemFile = GetSystemFile();
                _systemFile.MetadataFile = null;

                Assert.ThrowsException<ArgumentNullException>(() => _systemFile.TrySave());
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void Equals_Test_ReturnsTrueGivenEqualSystemFile()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();
                var systemFile1 = GetSystemFile();
                var systemFile2 = GetSystemFile();

                var areEqual = systemFile1.Equals(systemFile2);

                Assert.IsTrue(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }

        [TestMethod]
        public void Equals_Test_ReturnsFalseGivenNotEqualSystemFile()
        {
            try
            {
                FileTestSharedVariables.CopyFileToTestDir();

                var systemFile1 = GetSystemFile();
                var systemFile2 = GetSystemFile();
                systemFile2.Extension = ".SomeDifferentExtension";

                var areEqual = systemFile1.Equals(systemFile2);

                Assert.IsFalse(areEqual);
            }
            finally
            {
                FileTestSharedVariables.DeleteTestDirectory();
            }
        }
    }
}
