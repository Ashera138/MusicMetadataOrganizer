using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.API
{
    [TestClass]
    public class GracenoteAPI_Tests
    {
        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void Query_Test_EmptyMetadataReturnsNullALBUM()
        {
            var metadataFile = new MetadataFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3")
            {
                Artist = string.Empty,
                Album = string.Empty,
                Title = string.Empty
            };

            var response = GracenoteAPI.Query(metadataFile);
        }

        [TestMethod]
        public void Query_Test_ParsedXmlHasNoNullValues()
        {
            var metadataFile = new MetadataFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");

            var response = GracenoteAPI.Query(metadataFile);

            bool nullValuesExist = false;
            if (response.Artist == null || response.Album == null || response.Title == null ||
                response.Year == 0 || response.Genres == null || response.TrackNo == 0)
                nullValuesExist = true;
            Assert.IsFalse(nullValuesExist);
        }
    }
}
