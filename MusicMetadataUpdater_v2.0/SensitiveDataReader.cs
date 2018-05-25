using System.Collections.Generic;
using System.Xml.Linq;

namespace MusicMetadataUpdater_v2._0
{
    public static class SensitiveDataReader
    {
        private static XDocument xdoc;

        private static void LoadFile()
        {
            if (xdoc == null)
                xdoc = XDocument.Load(@"D:\My Documents\MusicMetadataConnectionInfo.xml");
        }

        public static Dictionary<string, string> GetConnectionCredentials()
        {
            LoadFile();
            var credentials = xdoc.Root.Element("credentials");
            var clientId = credentials.Element("clientId").Value;
            var userId = credentials.Element("userId").Value;

            return new Dictionary<string, string>()
            {
                { "clientId", clientId },
                { "userId", userId }
            };
        }
        
        public static string GetDestinationUrl()
        {
            LoadFile();
            var destinationUrl = xdoc.Root.Element("destinationUrl").Value;
            return destinationUrl;
        }
    }
}
