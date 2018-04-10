using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MusicMetadataUpdater_v2._0
{
    internal static class XmlParser
    {
        internal static RESPONSE XmlToRESPONSEObject(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<RESPONSE>), new XmlRootAttribute("RESPONSES"));
            using (var stringReader = new StringReader(xml))
            {
                var results = (List<RESPONSE>)serializer.Deserialize(stringReader);
                return results[0];
            }
        }
    }
}