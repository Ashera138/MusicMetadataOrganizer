using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class Group_Replace_Tests
    {
        private readonly string _input = "ABC";

        private Group GetFirstGroupFromRegexMatch()
        {
            var regex = new Regex("A");
            var match = regex.Matches(_input)[0];
            return match.Groups[0];
        }

        [TestMethod]
        public void Replace_Test_GroupIsReplacedGivenValidInput()
        {
            var group = GetFirstGroupFromRegexMatch();

            var result = group.Replace(_input, "0");

            Assert.IsTrue(result.Equals("0BC"));
        }

        [TestMethod]
        public void Replace_Test_RemovesInputGivenEmptyReplacementString()
        {
            var group = GetFirstGroupFromRegexMatch();

            var result = group.Replace(_input, "");

            Assert.IsTrue(result.Equals("BC"));
        }

        [TestMethod]
        public void Replace_Test_ThrowsArgumentNullExceptionGivenNullReplacementString()
        {
            var group = GetFirstGroupFromRegexMatch();

            Assert.ThrowsException<ArgumentNullException>(() => group.Replace(_input, null));
        }

        [TestMethod]
        public void Replace_Test_ThrowsArgumentNullExceptionGivenNullSourceString()
        {
            var group = GetFirstGroupFromRegexMatch();

            Assert.ThrowsException<NullReferenceException>(() => group.Replace(null, "0"));
        }

        [TestMethod]
        public void Replace_Test_ThrowsNullReferenceExceptionWhenGivenNullGroup()
        {
            Group nullGroup = null;

            Assert.ThrowsException<NullReferenceException>(() => nullGroup.Replace(_input, "0"));
        }

        [TestMethod]
        public void Replace_Test_ThrowsArgumentOutOfRangeExceptionGivenEmptySourceString()
        {
            var group = GetFirstGroupFromRegexMatch();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => group.Replace("", "0"));
        }
    }
}
