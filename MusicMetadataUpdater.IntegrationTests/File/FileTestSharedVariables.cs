using System;
using System.IO;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    internal static class FileTestSharedVariables
    {
        internal const string baseTestDirectory = @"C:\_TempForTesting\";
        internal const string testDirectory = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\";
        internal const string originalFilepath = 
            @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\The Adventure.mp3";
        internal const string copyOfOriginalFilepath = 
            @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\The Adventure Copy.mp3";

        internal static void CopyFileToTestDir(string destinationPath = originalFilepath)
        {
            CreateTestDirectory();
            System.IO.File.Copy($@"C:\Users\Ashie\Desktop\The Adventure.mp3", destinationPath);
        }

        internal static void CreateTestDirectory(string testDirectory = testDirectory)
        {
            if (!Directory.Exists(testDirectory))
                Directory.CreateDirectory(testDirectory);
        }

        internal static void DeleteTestDirectory(string testDirectory = baseTestDirectory)
        {
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }
}
