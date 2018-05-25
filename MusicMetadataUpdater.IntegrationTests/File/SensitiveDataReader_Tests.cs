using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUpdater.IntegrationTests.File
{
    [TestClass]
    public class SensitiveDataReader_Tests
    {
        [TestMethod]
        public void GetDestinationUrl_Test_ReturnsValidUri()
        {
            var destinationUrl = SensitiveDataReader.GetDestinationUrl();
            Assert.IsTrue(Uri.IsWellFormedUriString(destinationUrl, UriKind.Absolute));
        }

        [TestMethod]
        public void GetConnectionCredentials_Test_ReturnsPopulatedValues()
        {
            var credentials = SensitiveDataReader.GetConnectionCredentials();
            Assert.IsFalse(string.IsNullOrEmpty(credentials["clientId"]) &&
                          (string.IsNullOrEmpty(credentials["userId"])));
        }
    }
}
