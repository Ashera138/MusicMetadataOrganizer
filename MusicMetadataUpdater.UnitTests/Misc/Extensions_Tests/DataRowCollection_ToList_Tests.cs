using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater.UnitTests.Database;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class DataRowCollection_ToList_Tests
    {
        MockMetadataFile _mockMetadataFile;
        DataTable _tableForMockMetadataFile;
        
        private void PopulateTestingFields()
        {
            _mockMetadataFile = TestClassFactory.GetMockMetadataFileWithSomeNullDummyValues();
            _tableForMockMetadataFile = TestClassFactory.GetDataTable(_mockMetadataFile);
        }

        [TestMethod]
        public void ToList_Test_ReturnsNotNullListGivenValidInput()
        {
            PopulateTestingFields();

            var list = _tableForMockMetadataFile.Rows.ToList<MockMetadataFile>();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void ToList_Test_ReturnsPopulatedListGivenInputWithNullablePropertyTypes()
        {
            var numOfObjects = 5;
            var table = TableMaker.GenerateDataTable<MockMetadataFile>(numOfObjects);

            var list = table.Rows.ToList<MockMetadataFile>();

            Assert.IsTrue(list.Count == numOfObjects);
        }

        [TestMethod]
        public void ToList_Test_ReturnsPopulatedListGivenInputWithNoNullablePropertyTypes()
        {
            var numOfObjects = 5;
            var table = TableMaker.GenerateDataTable<MockSystemFile>(numOfObjects);

            var list = table.Rows.ToList<MockSystemFile>();

            Assert.IsTrue(list.Count == numOfObjects);
        }

        [TestMethod]
        public void ToList_Test_ReturnsMatchingObjectGivenInputWithNullNullableTypes()
        {
            PopulateTestingFields();

            var list = _tableForMockMetadataFile.Rows.ToList<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFile.Equals(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsMatchingObjectGivenInputWithNotNullNullableTypes()
        {
            _mockMetadataFile = TestClassFactory.GetMockMetadataFileWithNoNullDummyValues();
            _tableForMockMetadataFile = TestClassFactory.GetDataTable(_mockMetadataFile);

            var list = _tableForMockMetadataFile.Rows.ToList<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFile.Equals(list[0]));
        }

        [TestMethod]
        public void ToList_Test_ReturnsEmptyCollectionGivenEmptyInput()
        {
            var table = new DataTable("tableName");

            var list = table.Rows.ToList<MockSystemFile>();

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void ToList_Test_ThrowsArgumentExceptionGivenUnparsableInput()
        {
            var table = TableMaker.GenerateDataTable<MockSystemFile>(1);

            Assert.ThrowsException<ArgumentException>(() => table.Rows.ToList<MockMetadataFile>());
        }

        [TestMethod]
        public void ToList_Test_ThrowsArgumentNullExceptionGivenNullInput()
        {
            DataRowCollection nullRows = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullRows.ToList<MockSystemFile>());
        }

        [TestMethod]
        public void ToList_Test_ReturnsListWithSameSizeAsInput()
        {
            Random random = new Random();
            var numOfRowsToGenerate = random.Next(1, 100);
            DataTable table = TableMaker.GenerateDataTable<MockMetadataFile>(numOfRowsToGenerate);

            var list = table.Rows.ToList<MockMetadataFile>();

            Assert.AreEqual(list.Count, numOfRowsToGenerate);
        }
    }
}
