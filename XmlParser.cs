using System;
using System.Xml.Linq;

namespace MusicMetadataUpdater_v2._0
{
    internal static class XmlParser
    {
        internal static GracenoteAPIResponse ParseXml(string xml)
        {
            try
            {
                var response = XDocument.Parse(xml).Root.Element("RESPONSE").Element("ALBUM");
                var artist = response.Element("ARTIST").Value;
                var album = response.Element("TITLE").Value;
                var title = response.Element("TRACK").Element("TITLE").Value;
                var year = Convert.ToUInt32(response.Element("DATE").Value);
                var genre = response.Element("GENRE").Value;
                var track = Convert.ToUInt32(response.Element("TRACK").Element("TRACK_NUM").Value);

                return new GracenoteAPIResponse()
                {
                    Artist = artist,
                    Album = album,
                    Title = title,
                    Year = year,
                    Genre = genre,
                    TrackNo = track
                };
            }
            catch (Exception ex)
            {
                LogWriter.Write($"XmlParse.ParseXml() Could not parse the GracenoteAPI XML response. {ex.GetType()}: \"{ex.Message}\"");
                throw;
            }
        }
    }
}