using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class XElement_ElementValueNull_Tests
    {
        [TestMethod]
        public void ElementValueNull_Test_ReturnsLongGivenNotNullXElement()
        {
            XElement xElement = new XElement("ElementName", 0);

            var result = xElement.ElementValueNull();

            Assert.IsTrue(result.GetType() == typeof(long));
        }

        [TestMethod]
        public void ElementValueNull_Test_ReturnsZeroGivenNullXElement()
        {
            XElement xElement = null;

            var result = xElement.ElementValueNull();

            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void ElementValueNull_Test_ThrowsFormatExceptionGivenStringXElementValue()
        {
            XElement xElement = new XElement("ElementName", "string value");

            Assert.ThrowsException<FormatException>(() => xElement.ElementValueNull());
        }

        [TestMethod]
        public void ElementValueNull_Test_ThrowsFormatExceptionGivenNumberLargerThanLongAsXElementValue()
        {
            double largeNumber = double.MaxValue;
            XElement xElement = new XElement("ElementName", largeNumber);

            Assert.ThrowsException<FormatException>(() => xElement.ElementValueNull());
        }

        [TestMethod]
        public void ElementValueNull_Test_ThrowsFormatExceptionGivenNumberSmallerThanLongAsXElementValue()
        {
            double smallNumber = double.MinValue;
            XElement xElement = new XElement("ElementName", smallNumber);

            Assert.ThrowsException<FormatException>(() => xElement.ElementValueNull());
        }
    }
}
