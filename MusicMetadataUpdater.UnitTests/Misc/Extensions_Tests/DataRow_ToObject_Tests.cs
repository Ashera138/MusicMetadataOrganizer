using System;
using System.Data;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater.UnitTests.Database;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class DataRow_ToObject_Tests
    {
        MockMetadataFile _mockMetadataFile;
        MockSystemFile _mockSystemFile;
        DataTable _tableForMockMetadataFile;
        DataTable _tableForMockSystemFile;

        private void PopulateTestingFields()
        {
            _mockMetadataFile = TestClassFactory.GetMockMetadataFileWithSomeNullDummyValues();
            _mockSystemFile = TestClassFactory.GetMockSystemFileWithDummyValues();
            _tableForMockMetadataFile = TestClassFactory.GetDataTable(_mockMetadataFile);
            _tableForMockSystemFile = TestClassFactory.GetDataTable(_mockSystemFile);
        }

        [TestMethod]
        public void ToObject_Test_ReturnsNotNullObjectGivenValidInput()
        {
            var table = TableMaker.GenerateDataTable<MockSystemFile>(1);

            var returnedObject = table.Rows[0].ToObject<MockSystemFile>();

            Assert.IsNotNull(returnedObject);
        }

        [TestMethod]
        public void ToObject_Test_ReturnsPopulatedObjectGivenInputWithNullablePropertyTypes()
        {
            PopulateTestingFields();   

            var returnedObject = _tableForMockMetadataFile.Rows[0].ToObject<MockMetadataFile>();

            Assert.IsNotNull(returnedObject);
        }

        [TestMethod]
        public void ToObject_Test_ReturnsPopulatedObjectGivenInputWitNoNullablePropertyTypes()
        {
            PopulateTestingFields();

            var returnedObject = _tableForMockSystemFile.Rows[0].ToObject<MockSystemFile>();

            Assert.IsNotNull(returnedObject);
        }

        [TestMethod]
        public void ToObject_Test_ReturnsMatchingObjectGivenInputWithNullablePropertyTypes()
        {
            PopulateTestingFields();

            var returnedObject = _tableForMockMetadataFile.Rows[0].ToObject<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFile.Equals(returnedObject));
        }

        [TestMethod]
        public void ToObject_Test_ReturnsMatchingObjectGivenInputWithNoNullablePropertyTypes()
        {
            var mockSystemFile = TestClassFactory.GetMockSystemFileWithDummyValues();
            var table = TestClassFactory.GetDataTable(mockSystemFile);

            var returnedObject = table.Rows[0].ToObject<MockSystemFile>();

            Assert.IsTrue(mockSystemFile.Equals(returnedObject));
        }

        [TestMethod]
        public void ToObject_Test_ReturnsObjectWithDefaultPropertyValuesGivenEmptyInput()
        {
            var emptyTable = new DataTable();

            var returnedObject = emptyTable.NewRow().ToObject<MockSystemFile>();

            Assert.IsTrue(HelperMethodsForTesting.PropertiesAreSetToDefaultValues(returnedObject));
        }

        [TestMethod]
        public void ToObject_Test_ThrowsArgumentExceptionGivenUnparsableInput()
        {
            PopulateTestingFields();

            Assert.ThrowsException<ArgumentException>(() => _tableForMockMetadataFile.Rows[0].ToObject<MockSystemFile>());
        }

        [TestMethod]
        public void ToObject_Test_ThrowsArgumentNullExceptionGivenNullInput()
        {
            DataRow nullRow = null;

            Assert.ThrowsException<ArgumentNullException>(() => nullRow.ToObject<MockSystemFile>());
        }
    }
}
