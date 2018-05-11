using System;
using System.Xml.Linq;

namespace MusicMetadataUpdater_v2._0
{
    internal static class XmlParser
    {
        internal static GracenoteAPIResponse ParseXml(string xml)
        {
            // Might need to changed from escaped XML to the normal chars
            try
            {
                var response = XDocument.Parse(xml).Root.Element("RESPONSE").Element("ALBUM");
                var artist = response.Element("ARTIST").Value;
                var album = response.Element("TITLE").Value;
                var title = response.Element("TRACK").Element("TITLE").Value;
                var year = response.Element("DATE").ElementValueNull();
                //var year = response.Element("DATE").ElementValueNull();
                var genre = response.Element("GENRE").Value;
                var track = response.Element("TRACK").Element("TRACK_NUM").ElementValueNull();

                return new GracenoteAPIResponse()
                {
                    Artist = artist,
                    Album = album,
                    Title = title,
                    Year = year,
                    Genres = genre,
                    TrackNo = track
                };
            }
            catch (Exception ex)
            {
                LogWriter.Write($"XmlParse.ParseXml() Could not parse the GracenoteAPI XML response. {ex.GetType()}: " +
                    $"\"{ex.Message}\" \n XML that threw the exception: {xml}");
                throw;
            }
        }
    }
}