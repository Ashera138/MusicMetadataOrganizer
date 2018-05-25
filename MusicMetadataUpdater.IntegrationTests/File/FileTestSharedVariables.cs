using System.IO;

namespace MusicMetadataUpdater.IntegrationTests
{
    internal static class FileTestSharedVariables
    {
        internal const string originalFilepath = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\The Adventure.mp3";
        internal const string copyOfOriginalFilepath = @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\The Adventure Copy.mp3";

        internal static void CopyFileToTestDir(string destinationPath = originalFilepath)
        {
            CreateTestDirectory();
            System.IO.File.Copy($@"C:\Users\Ashie\Desktop\The Adventure.mp3", destinationPath);
        }

        private static void CreateTestDirectory(string testDirectory = 
            @"C:\_TempForTesting\Angels and Airwaves\We Don't Need to Whisper\")
        {
            if (!Directory.Exists(testDirectory))
                Directory.CreateDirectory(testDirectory);
        }

        internal static void DeleteTestDirectory(string testDirectory =
            @"C:\_TempForTesting\")
        {
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }
}
