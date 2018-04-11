using System;
using System.IO;
using System.Reflection;

namespace MusicMetadataUpdater_v2._0
{
    public static class LogWriter
    {
        private static string app_exePath;

        static LogWriter()
        {
            app_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void Write(string logMessage)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(app_exePath, "log.txt")))
                {
                    AppendMessageToLogWithTextWriter(logMessage, writer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create an error log in {app_exePath}. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private static void AppendMessageToLogWithTextWriter(string logMessage, TextWriter txtWriter)
        {
            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            txtWriter.WriteLine("  :");
            txtWriter.WriteLine($"  :{logMessage}");
            txtWriter.WriteLine("-------------------------------");
        }
    }
}
