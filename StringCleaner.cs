using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    public static class StringCleaner
    {
        public static string RemoveInvalidDirectoryCharacters(string directory)
        {
            char[] invalidPathChars = Path.GetInvalidPathChars();
            var sanitizedDirectory = ReplaceCharactersWithHyphen(directory, ':');
            sanitizedDirectory = RemoveInvalidChars(sanitizedDirectory, invalidPathChars);
            return sanitizedDirectory;
        }

        public static string RemoveInvalidFileNameCharacters(string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            var sanitizedFileName = ReplaceCharactersWithHyphen(fileName, ':', '/');
            sanitizedFileName = RemoveInvalidChars(sanitizedFileName, invalidFileNameChars);
            return sanitizedFileName;
        }

        private static string ReplaceCharactersWithHyphen(string input, params char[] charactersToReplace)
        {
            var result = input;
            foreach (char c in charactersToReplace)
            {
                result = result.Replace(c, '-');
            }
            return result;
        }

        private static string RemoveInvalidChars(string input, char[] invalidChars)
        {
            var sanitizedInput = input;
            foreach (char invalidChar in invalidChars)
            {
                if (sanitizedInput.Contains(invalidChar))
                    sanitizedInput = sanitizedInput.Replace(invalidChar.ToString(), string.Empty);
            }
            return sanitizedInput;
        }
    }
}
