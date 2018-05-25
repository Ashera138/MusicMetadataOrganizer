using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataUpdater.UnitTests.Database;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class DataTable_ToListAsync_Tests
    {
        MockMetadataFile _mockMetadataFileWithNullValues = TestClassFactory.GetMockMetadataFileWithDummyValues();
        MockMetadataFile _mockMetadataFileWithNoNullValues = TestClassFactory.GetMockMetadataFileWithNoNullDummyValues();
        DataTable _table;

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsNotNullListGivenValidInput()
        {
            var table = TableMaker.GenerateDataTable<MockMetadataFile>(5);

            var list = await table.ToListAsync<MockMetadataFile>();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsPopulatedObjectInListGivenInputWithNullNullablePropertyTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNullValues);

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.IsTrue(HelperMethodsForTesting.HasANullablePropertyWithNullValue(list[0]));
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsPopulatedObjectInListGivenInputWithNotNullNullablePropertyTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNoNullValues);

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.IsFalse(HelperMethodsForTesting.HasANullablePropertyWithNullValue(list[0]));
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsPopulatedObjectInListGivenInputWithNoNullablePropertyTypes()
        {
            var mockSystemFile = TestClassFactory.GetMockSystemFileWithDummyValues();
            _table = TestClassFactory.GetDataTable(mockSystemFile);

            var list = await _table.ToListAsync<MockSystemFile>();

            Assert.IsFalse(HelperMethodsForTesting.PropertiesAreSetToDefaultValues(list[0]));
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsMatchingObjectGivenInputWithNullNullableTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNullValues);

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFileWithNullValues.Equals(list[0]));
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsMatchingObjectGivenInputWithNotNullNullableTypes()
        {
            _table = TestClassFactory.GetDataTable(_mockMetadataFileWithNoNullValues);

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.IsTrue(_mockMetadataFileWithNoNullValues.Equals(list[0]));
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsEmptyListGivenEmptyInput()
        {
            _table = TableMaker.GenerateDataTable<MockMetadataFile>(1);
            _table.Clear();

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.AreEqual(list.Count, 0);
        }

        [TestMethod]
        public async Task ToListAsync_Test_ThrowsArgumentNullExceptionGivenNullInput()
        {
            _table = null;

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _table.ToListAsync<MockMetadataFile>());
        }

        [TestMethod]
        public void ToListAsync_Test_TaskReturnsNoExceptions()
        {
            _table = TableMaker.GenerateDataTable<MockMetadataFile>(5);

            var taskResult = _table.ToListAsync<MockMetadataFile>();

            Assert.IsNull(taskResult.Exception);
        }

        [TestMethod]
        public async Task ToListAsync_Test_ReturnsListWithSameSizeAsInput()
        {
            Random random = new Random();
            var numOfRowsToGenerate = random.Next(1, 100);
            _table = TableMaker.GenerateDataTable<MockMetadataFile>(numOfRowsToGenerate);

            var list = await _table.ToListAsync<MockMetadataFile>();

            Assert.AreEqual(list.Count, numOfRowsToGenerate);
        }
    }
}
