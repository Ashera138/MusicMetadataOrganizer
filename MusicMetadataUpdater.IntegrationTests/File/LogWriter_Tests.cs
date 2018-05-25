using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class LogWriter_Tests
    {
        private readonly string logPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log.txt");

        // Reads and returns the end of the log file so the entire file isn't loaded into memory.
        private string GetLastLogEntries()
        {
            const byte commonEntrySizeInBytes = 183;
            string endOfLog = string.Empty;
            using (var reader = new StreamReader(logPath))
            {
                if (reader.BaseStream.Length > commonEntrySizeInBytes)
                {
                    reader.BaseStream.Seek((-commonEntrySizeInBytes), SeekOrigin.End);
                    endOfLog = reader.ReadToEnd();
                }
            }
            return endOfLog;
        }

        [TestMethod]
        public void Write_Test_LogExistsAfterWriting()
        {
            LogWriter.Write("Test entry from LW_Write_Test_LogExistsAfterWriting() unit test.");

            if (!System.IO.File.Exists(logPath))
                Assert.Fail("Log file does not exist.");
        }

        [TestMethod]
        public void Write_Test_EntryExistsInLogFile()
        {
            var currentTime = DateTime.Now.ToLongTimeString();
            LogWriter.Write($"Test entry from LW_Write_Test_EntryExistsInLogFile() unit test. Current time: {currentTime}");

            var endOfLog = GetLastLogEntries();

            if (!endOfLog.Contains(currentTime))
                Assert.Fail("The entry does not exist in the log file.");
        }
    }
}
