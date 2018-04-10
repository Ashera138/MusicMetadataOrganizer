using System;
using System.IO;
using System.Net;
using System.Text;

namespace MusicMetadataUpdater_v2._0
{
    public static class GracenoteAPI
    {
        public static RESPONSE Query(MetadataFile metadataFile, bool includeAlbumInQuery = true)
        {
            var xmlToPost = XmlGenerator.CreateXmlToPost(metadataFile, includeAlbumInQuery);
            var result = PostXmlData(xmlToPost);
            if (string.IsNullOrEmpty(result))
            {
                LogWriter.Write($"GracenoteAPI.Query() - Received a null result from the PostXMLData() method. " +
                    $"ArgumentNullException: Application terminated.");
                throw new ArgumentNullException(nameof(result));
            }
            return XmlParser.XmlToRESPONSEObject(result);
        }

        private static string PostXmlData(string xmlToPost)
        {
            var responseText = string.Empty;
            HttpWebRequest request = CreateWebRequest();
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(xmlToPost);
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    responseText = new StreamReader(responseStream).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write($"GracenoteAPI.PostXmlData() - Could not connect to the Gracenote web API. " +
                    $"{ex.GetType()}: \"{ex.Message}\"");
            }
            return responseText;
        }

        private static HttpWebRequest CreateWebRequest()
        {
            var destinationUrl = SensitiveDataReader.GetDestinationUrl();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Method = "POST";
            return request;
        }
    }
}
