using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicMetadataUpdater_v2._0;

namespace MetadataFileTests.Misc.Extensions_Tests
{
    [TestClass]
    public class DbSet_Clear_Tests
    {
        // Have not refactored yet
        // TODO: Conditions to test:
        // -- Succesfully clears with populated dbSet
        // -- Returns empty when given empty dbSet

        /*
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
        */

        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(m => m.Local).Returns(new ObservableCollection<T>(sourceList));

            return dbSet.Object;
        }

        // Not working
        [TestMethod]
        public void Clear_Test_ReturnsEmptyDbSetGivenPopulatedMetadataFileDbSet()
        {
            // with moq
            FileDb fileContext = new FileDb
            {
                MetadataFiles = GetQueryableMockDbSet(
                new MetadataFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3"),
                new MetadataFile(@"C:\Users\Ashie\Desktop\Going Away to College.mp3"))
            };

            Assert.IsTrue(fileContext.MetadataFiles.Count() > 0);

            fileContext.MetadataFiles.Clear();

            Assert.IsTrue(fileContext.MetadataFiles.Count() == 0);
        }
    }
}
