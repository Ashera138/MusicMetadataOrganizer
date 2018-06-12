using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetadataFileTests.Misc
{
    [TestClass]
    public class XmlGenerator_Tests
    {
        /*
        internal static string CreateXmlToPost(MetadataFile metadataFile, bool includeAlbumInQuery = true)
        {
            var artist = metadataFile.Artist;
            var title = metadataFile.Title;
            var album = metadataFile.Album;
            return includeAlbumInQuery ? CreateRequest(artist, title, album) : CreateRequest(artist, title);
        }
        */

        [TestMethod]
        public void CreateXmlToPost_Test_ReturnsNotNullString()
        {

        }
    }
}
