using System;
using System.IO;
using System.Net;
using System.Text;

namespace MusicMetadataUpdater_v2._0
{
    public static class GracenoteAPI
    {
        private static System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        private const string destinationUrl = "https://c834201935.web.cddbp.net/webapi/xml/1.0/";

        public static RESPONSE Query(MetadataFile metadataFile, bool includeAlbumInQuery = true)
        {
            var artist = metadataFile.Artist;
            var songTitle = metadataFile.Title;
            var album = metadataFile.Album;
            var xml = string.Empty;
            if (includeAlbumInQuery)
                xml = XmlGenerator.CreateRequest(artist, songTitle, album);
            else
                xml = XmlGenerator.CreateRequest(artist, songTitle);
            var result = PostXmlData(xml);
            if (String.IsNullOrEmpty(result))
            {
                // Change this message - (class name)
                LogWriter.Write($"GracenoteWebAPI.Query() - Received a null result from the PostXMLData() method. " +
                    $"ArgumentNullException: Application terminated.");
                throw new ArgumentNullException(nameof(result));
            }
            return XmlParser.XmlToObject(result)[0];
        }

        private static string PostXmlData(string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes = Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
            }
            catch (Exception ex)
            {
                // Change this message - (class name)
                LogWriter.Write($"GracenoteWebAPI.PostXMLData() - Could not connect to the Gracenote web API. " +
                    $"{ex.GetType()}: \"{ex.Message}\"");
            }
            return null;
        }
    }
}
