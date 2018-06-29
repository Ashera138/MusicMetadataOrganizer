using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.API
{
    [TestClass]
    public class GracenoteAPI_Tests
    {
        [TestMethod]
        public void Query_Test_ReturnsNonNullResultGivenValidInput()
        {
            var metadataFile = GetMetadataFile();

            var response = GracenoteAPI.Query(metadataFile);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Query_Test_ParsedXmlHasNoNullValuesGivenValidInput()
        {
            var metadataFile = GetMetadataFile();

            var response = GracenoteAPI.Query(metadataFile);

            Assert.IsFalse(HasNullValues(response));
        }

        [TestMethod]
        public void Query_Test_ThrowsNullReferenceExceptionGivenEmptyFieldsInInput()
        {
            var metadataFile = GetMetadataFile();
            metadataFile.Artist = string.Empty;
            metadataFile.Album = string.Empty;
            metadataFile.Title = string.Empty;

            Assert.ThrowsException<NullReferenceException>(() => GracenoteAPI.Query(metadataFile));
        }

        [TestMethod]
        public void Query_Test_ThrowsNullReferenceExceptionGivenNullInput()
        {
            MetadataFile nullFile = null;

            Assert.ThrowsException<NullReferenceException>(() => GracenoteAPI.Query(nullFile));
        }

        private MetadataFile GetMetadataFile()
        {
            var filepath = @"C:\Users\Ashie\Desktop\The Adventure.mp3";
            return new MetadataFile(filepath);
        }

        private bool HasNullValues(GracenoteAPIResponse response)
        {
            bool nullValuesExist = false;

            if (response.Artist == null || response.Album == null || response.Title == null ||
                response.Year == 0 || response.Genres == null || response.TrackNo == 0)
                nullValuesExist = true;

            return nullValuesExist;
        }
    }
}
