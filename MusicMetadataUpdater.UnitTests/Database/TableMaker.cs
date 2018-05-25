using FizzWare.NBuilder;
using System.Data;
using System.Linq;

namespace MusicMetadataUpdater.UnitTests.Database
{
    internal static class TableMaker
    {
        internal static DataTable GenerateDataTable<T>(int rows)
        {
            var datatable = new DataTable(typeof(T).Name);
            typeof(T).GetProperties().ToList().ForEach(
                x => datatable.Columns.Add(x.Name));
            Builder<T>.CreateListOfSize(rows).Build().ToList().ForEach(
                x => datatable.LoadDataRow(x.GetType().GetProperties().Select(
                    y => y.GetValue(x, null)).ToArray(), true));
            return datatable;
        }
    }
}
