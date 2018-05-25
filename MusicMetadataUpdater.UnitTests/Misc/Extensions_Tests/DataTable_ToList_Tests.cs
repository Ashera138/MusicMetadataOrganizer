using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater.UnitTests.Database;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class DataTable_ToList_Tests
    {
        MockMetadataFile _mockMetadataFileWithNullValues = TestClassFactory.GetMockMetadataFileWithDummyValues();
        MockMetadataFile _mockMetadataFileWithNoNullValues = TestClassFactory.GetMockMetadataFileWithNoNullDummyValues();
        DataTable _table;
    
        [TestMethod]
        public void ToList_Test_ReturnsNotNullListGivenValidInput()
        {
            var table = TableMaker.GenerateDataTable<MockMetadataFile>(5);

            var list = table.ToList<MockMetadataFile>();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void ToList_Test_ReturnsPopulatedObjectInListGivenInputWithNullNullablePropertyTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNullValues);

            var list = _table.ToList<MockMetadataFile>();

            Assert.IsTrue(HelperMethodsForTesting.HasANullablePropertyWithNullValue(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsPopulatedObjectInListGivenInputWithNotNullNullablePropertyTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNoNullValues);

            var list = _table.ToList<MockMetadataFile>();

            Assert.IsFalse(HelperMethodsForTesting.HasANullablePropertyWithNullValue(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsPopulatedObjectInListGivenInputWithNoNullablePropertyTypes()
        {
            var mockSystemFile = TestClassFactory.GetMockSystemFileWithDummyValues();
            _table = TestClassFactory.GetDataTable(mockSystemFile);

            var list = _table.ToList<MockSystemFile>();

            Assert.IsFalse(HelperMethodsForTesting.PropertiesAreSetToDefaultValues(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsMatchingObjectGivenInputWithNullNullableTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNullValues);

            var list = _table.ToList<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFileWithNullValues.Equals(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsMatchingObjectGivenInputWithNotNullNullableTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNoNullValues);

            var list = _table.ToList<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFileWithNoNullValues.Equals(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsEmptyListGivenEmptyInput()
        {
            var table = TableMaker.GenerateDataTable<MockMetadataFile>(1);
            table.Clear();

            var list = table.ToList<MockMetadataFile>();

            Assert.AreEqual(list.Count, 0);
        }

        [TestMethod]
        public void ToList_Test_ThrowsArgumentNullExceptionGivenNullInput()
        {
            DataTable table = null;

            Assert.ThrowsException<ArgumentNullException>(() => table.ToList<MockMetadataFile>());
        }

        [TestMethod]
        public void ToList_Test_ReturnsListWithSameSizeAsInput()
        {
            Random random = new Random();
            var numOfRowsToGenerate = random.Next(1, 100);
            var table = TableMaker.GenerateDataTable<MockMetadataFile>(numOfRowsToGenerate);

            var list = table.ToList<MockMetadataFile>();

            Assert.AreEqual(list.Count, numOfRowsToGenerate);
        }
    }
}
