using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class SystemFile_Tests
    {
        SystemFile systemFile;
        readonly string artist = "Angels and Airwaves";
        readonly string album = "We Don't Need to Whisper";
        readonly string song = "The Adventure.mp3";
        readonly string exceedsMaxCharString = "THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST " +
                                      "MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS " +
                                      "THIS IS A STRING TO TEST MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST " +
                                      "MAX CHAR PATH CONDITIONS THIS IS A STRING TO TEST MAX PATH CHAR CONDITIONS";

        private void PopulateTestingFields()
        {
            var filepath = $@"C:\_TempForTesting\{artist}\{album}\{song}";
            systemFile = new SystemFile(filepath);
            var metadataFile = new MetadataFile(filepath);
            systemFile.MetadataFile = metadataFile;
        }

        [TestMethod]
        public void MoveToCorrectArtistLocation_Test_ValidInput_FileIsMovedFromOriginalLocationAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Artist = "Test Artist";

            systemFile.MoveToCorrectArtistLocation();

            bool success = !System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The file still exists in its original location after being moved.");
        }

        [TestMethod]
        public void MoveToCorrectArtistLocation_Test_ValidInput_FileExistsInNewLocationAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Artist = "Test Artist";

            systemFile.MoveToCorrectArtistLocation();

            bool success = System.IO.File.Exists($@"C:\_TempForTesting\Test Artist\{album}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The file does not exist in the destination location after being moved.");
        }

        [TestMethod]
        public void MoveToCorrectArtistLocation_Test_WhenNewNameOnlyDiffersByCase()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Artist = "aNgElS aNd AiRwAvEs";
            var originalFilepath = systemFile.Filepath;

            systemFile.MoveToCorrectArtistLocation();

            var newFilePath = new FileInfo(systemFile.Filepath).FullName;
            bool success = (originalFilepath != newFilePath);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("Names are equal.");
        }

        [TestMethod]
        public void MoveToCorrectArtistLocation_Test_FilepathTooLong()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Artist = exceedsMaxCharString;

            systemFile.MoveToCorrectArtistLocation();

            bool success = !Directory.Exists($@"C:\_TempForTesting\{exceedsMaxCharString}\{album}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void MoveToCorrectAlbumLocation_Test_ValidInput_FileIsMovedFromOriginalLocationAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Album = "Test Album";

            systemFile.MoveToCorrectAlbumLocation();

            bool success = !System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The file still exists in its original location after being moved.");
        }

        [TestMethod]
        public void MoveToCorrectAlbumLocation_Test_ValidInput_FileExistsInNewLocationAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Album = "Test Album";

            systemFile.MoveToCorrectAlbumLocation();

            bool success = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\Test Album\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("The file does not exist in the destination location after being moved.");
        }

        [TestMethod]
        public void MoveToCorrectAlbumLocation_Test_WhenNewNameOnlyDiffersByCase()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Album = "wE dOn'T nEeD tO wHiSpEr";
            var originalFilepath = systemFile.Filepath;

            systemFile.MoveToCorrectAlbumLocation();

            var newFilePath = new FileInfo(systemFile.Filepath).FullName;
            bool success = (originalFilepath != newFilePath);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("Original and new filepath are equal.");
        }

        [TestMethod]
        public void MoveToCorrectAlbumLocation_Test_FilepathTooLong()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Album = exceedsMaxCharString;

            systemFile.MoveToCorrectAlbumLocation();

            bool success = !Directory.Exists($@"C:\_TempForTesting\{artist}\{exceedsMaxCharString}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("Directory that should have exceeded the max amount of characters in a path was actually created.");
        }

        [TestMethod]
        public void RenameFile_Test_ValidInput_FileDoesNotHaveOldNameAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Title = "Test name";

            systemFile.RenameFile();

            bool success = !System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\{song}");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void RenameFile_Test_ValidInput_FileHasNewNameAfterRename()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Title = "Test name";

            systemFile.RenameFile();

            bool success = System.IO.File.Exists($@"C:\_TempForTesting\{artist}\{album}\Test name.mp3");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void RenameFile_Test_WhenNewNameOnlyDiffersByCase()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Title = "tHe AdVeNtUrE";
            var originalFilepath = systemFile.Filepath;

            systemFile.RenameFile();

            var newFilePath = new FileInfo(systemFile.Filepath).FullName;
            bool success = (originalFilepath != newFilePath);
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail("Names are equal.");
        }

        [TestMethod]
        public void RenameFile_Test_FilepathTooLong()
        {
            FileTestSharedVariables.CopyFileToTestDir();
            PopulateTestingFields();
            systemFile.MetadataFile.Title = exceedsMaxCharString;

            systemFile.RenameFile();

            bool success = !Directory.Exists($@"C:\_TempForTesting\{album}\{exceedsMaxCharString}.mp3");
            FileTestSharedVariables.DeleteTestDirectory();
            if (!success)
                Assert.Fail();
        }

        [TestMethod]
        public void TrySave_Test_Condition()
        {
            // Determine test condition and update name ^
            Assert.Fail("Not yet implemented.");
        }

        [TestMethod]
        public void Equals_Test_ReturnsTrueWhenEqual()
        {
            var filepath = FileTestSharedVariables.originalFilepath;
            FileTestSharedVariables.CopyFileToTestDir(filepath);

            var systemFile1 = new MetadataFile(filepath);
            var systemFile2 = new MetadataFile(filepath);

            FileTestSharedVariables.DeleteTestDirectory();
            if (!systemFile1.Equals(systemFile2))
                Assert.Fail("Files are not equal.");
        }

        [TestMethod]
        public void Equals_Test_ReturnsFalseWhenNotEqual()
        {
            var filepath = FileTestSharedVariables.originalFilepath;
            FileTestSharedVariables.CopyFileToTestDir(filepath);

            var systemFile1 = new MetadataFile(filepath);
            var systemFile2 = new MetadataFile(filepath);
            systemFile2.Artist = "Something different";

            FileTestSharedVariables.DeleteTestDirectory();
            if (systemFile1.Equals(systemFile2))
                Assert.Fail("Files are not equal.");
        }
    }
}
