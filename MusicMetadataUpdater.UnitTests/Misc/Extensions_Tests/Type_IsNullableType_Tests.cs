using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class Type_IsNullableType_Tests
    {
        [TestMethod]
        public void IsNullableType_Test_ReturnsTrueGivenNullableType()
        {
            Type nullableType = typeof(int?);

            bool isNullableType = nullableType.IsNullableType();

            Assert.IsTrue(isNullableType);
        }

        [TestMethod]
        public void IsNullableType_Test_ReturnsFalseGivenBoxedNullableType()
        {
            int? nullableInt = 5;
            Type boxedNullableInt = nullableInt.GetType();

            bool isNullableType = boxedNullableInt.IsNullableType();
            
            Assert.IsFalse(isNullableType);
        }

        [TestMethod]
        public void IsNullableType_Test_ReturnsFalseGivenNonNullableType()
        {
            Type nonNullableType = typeof(long);

            bool isNullableType = nonNullableType.IsNullableType();

            Assert.IsFalse(isNullableType);
        }

        [TestMethod]
        public void IsNullableType_Test_ReturnsFalseGivenBoxedNonNullableType()
        {
            int nonNullableInt = 5;
            Type boxedNonNullableInt = nonNullableInt.GetType();

            bool isNullableType = boxedNonNullableInt.IsNullableType();

            Assert.IsFalse(isNullableType);
        }

        [TestMethod]
        public void IsNullableType_Test_ReturnsFalseGivenReferenceType()
        {
            Type referenceType = typeof(Random);

            bool isNullableType = referenceType.IsNullableType();

            Assert.IsFalse(isNullableType);
        }
    }
}
