using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.UnitTests.Misc
{
    [TestClass]
    public class StringCleaner_Tests
    {
        private static char[] invalidPathChars = Path.GetInvalidPathChars();
        private static char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        private const string sampleString = "This string will become a new directory folder.";
        private static Random random = new Random();

        private string InsertCharRandomlyIntoString(string input, char character)
        {
            var index = random.Next(0, input.Length - 1);
            return input.Insert(index, character.ToString());
        }

        [TestMethod]
        public void RemoveInvalidDirectoryCharacters_Test_InvalidCharsAreRemoved()
        {
            for (int i = 0; i < invalidPathChars.Length; i++)
            {
                var testString = InsertCharRandomlyIntoString(sampleString, invalidPathChars[i]);
                var result = StringCleaner.RemoveInvalidDirectoryCharacters(testString);
                if (!result.Equals(sampleString))
                    Assert.Fail($"Expected: {sampleString}, Actual: {result}. Char that failed test: {invalidPathChars[i]}.");
            }
        }

        [TestMethod]
        public void RemoveInvalidDirectoryCharacters_Test_SemicolonReplacedWithHyphen()
        {
            var testString = InsertCharRandomlyIntoString(sampleString, ':');

            Assert.AreEqual(StringCleaner.RemoveInvalidDirectoryCharacters(testString), 
                            testString.Replace(':', '-'));
        }

        [TestMethod]
        public void RemoveInvalidFileNameCharacters_Test_InvalidCharsAreRemoved()
        {
            var charactersToRemove = invalidFileNameChars.Where(c => (c != ':') && (c != '/')).ToList();

            for (int i = 0; i < charactersToRemove.Count(); i++)
            {
                var testString = InsertCharRandomlyIntoString(sampleString, charactersToRemove[i]);
                var result = StringCleaner.RemoveInvalidFileNameCharacters(testString);
                if (!result.Equals(sampleString))
                    Assert.Fail($"Expected: {sampleString}, Actual: {result}. Char that failed test: {charactersToRemove[i]}.");
            }
        }

        [TestMethod]
        public void RemoveInvalidFileNameCharacters_Test_SemiColonReplacedWithHyphen()
        {
            var testString = InsertCharRandomlyIntoString(sampleString, ':');

            Assert.AreEqual(StringCleaner.RemoveInvalidFileNameCharacters(testString),
                            testString.Replace(':', '-'));
        }

        [TestMethod]
        public void RemoveInvalidFileNameCharacters_Test_ForwardSlashReplacedWithHyphen()
        {
            var testString = InsertCharRandomlyIntoString(sampleString, '/');

            Assert.AreEqual(StringCleaner.RemoveInvalidDirectoryCharacters(testString),
                            testString.Replace(':', '-'));
        }
    }
}
