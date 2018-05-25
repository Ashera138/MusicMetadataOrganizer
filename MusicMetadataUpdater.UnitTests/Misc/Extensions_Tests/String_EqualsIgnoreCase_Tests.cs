using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class String_EqualsIgnoreCase_Tests
    {
        private readonly string _lowercaseText = "test";

        [TestMethod]
        public void EqualsIgnoreCase_Test_ReturnsTrueGivenStringsThatAreEqualExceptCase()
        {
            var areEqualIgnoringCase = _lowercaseText.EqualsIgnoreCase(_lowercaseText.ToUpper());

            Assert.IsTrue(areEqualIgnoringCase);
        }

        [TestMethod]
        public void EqualsIgnoreCase_Test_ReturnsTrueWhenStringIsComparedToItself()
        {
            var isEqual = _lowercaseText.EqualsIgnoreCase(_lowercaseText);

            Assert.IsTrue(isEqual);
        }

        public void EqualsIgnoreCase_Test_ReturnsTrueGivenTwoIdenticalStrings()
        {
            var copyOf_lowercaseText = string.Copy(_lowercaseText);

            var areEqual = _lowercaseText.EqualsIgnoreCase(copyOf_lowercaseText);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void EqualsIgnoreCase_Test_ReturnsFalseGivenUnequalStrings()
        {
            var areEqual = _lowercaseText.EqualsIgnoreCase("Different text");

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void EqualsIgnoreCase_Test_ReturnsFalseGivenEmptyAndPopulatedStrings()
        {
            var string1 = string.Empty;

            var areEqual = string1.EqualsIgnoreCase(_lowercaseText);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void EqualsIgnoreCase_Test_ReturnsTrueGivenTwoEmptyStrings()
        {
            var string1 = string.Empty;
            var string2 = string.Empty;

            var areEqual = string1.EqualsIgnoreCase(string2);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void EqualsIgnoreCase_Test_ThrowsNullReferenceExceptionGivenNullInput()
        {
            string string1 = null;

            Assert.ThrowsException<NullReferenceException>(() => string1.EqualsIgnoreCase(_lowercaseText));
        }
    }
}
