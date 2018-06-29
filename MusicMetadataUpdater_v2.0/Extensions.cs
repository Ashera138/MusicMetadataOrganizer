using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace MusicMetadataUpdater_v2._0
{
    public static class Extensions
    {
        public static bool EqualsIgnoreCase(this string string1, string string2)
        {
            return string1.Equals(string2, StringComparison.OrdinalIgnoreCase);
        }

        public static string Replace(this Group group, string source, string replacement)
        {
            AssertNonNull(replacement, "replacement");

            return source.Substring(0, group.Index) + replacement + source.Substring(group.Index + group.Length);
        }

        public static long ElementValueNull(this XElement element)
        {
            return element == null ? 0 : Convert.ToInt64(element.Value);
        }

        // I don't know how to unit test this method
        // No matter what mock options I try, it throws NotImplementedExceptions
        // ((System.Linq.IQueryable)dbSet).ElementType, Expression, and Provider
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            AssertNonNull(table.AsEnumerable(), "table");

            return table.Rows.ToList<T>();
        }

        private static void AssertNonNull(this object value, string paramName)
        {
            if (value == null)
            {
                LogWriter.Write($"DataRowCollection.ToIEnumerableOfObject<T>() Extension method - " +
                     $"Cannot parse a null DataRowCollection object. Row object: {value}. ArgumentNullException thrown.");

                throw new ArgumentNullException("Value must not be null.", paramName);
            }
        }

        public static List<T> ToList<T>(this DataRowCollection rows) where T : class, new()
        {
            return rows.Cast<DataRow>().ToList<T>();
        }

        private static List<T> ToList<T>(this IEnumerable<DataRow> rows) where T : class, new()
        {
            return rows.Select(r => r.ToObject<T>()).ToList();
        }

        public static async Task<List<T>> ToListAsync<T>(this DataTable table) where T : class, new()
        {
            return await Task.Run(() => table.ToList<T>());
        }

        public static T ToObject<T>(this DataRow row) where T : new()
        {
            AssertNonNull(row, "row");

            T item = new T();
            Type typeOfT = item.GetType();
            var index = 0;

            try
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo property = typeOfT.GetProperty(column.ColumnName);
                    Type propertyType = property.PropertyType;
                    object result = null;

                    if (propertyType.IsNullableType())
                    {
                        var rowIndex = row.Table.Rows.IndexOf(row);
                        var valueOfPropInfo = row.Table.Rows[rowIndex].ItemArray[index];
                        Type underlyingType = Nullable.GetUnderlyingType(propertyType);
                        result = (valueOfPropInfo == null || valueOfPropInfo == DBNull.Value) ? null : Convert.ChangeType(valueOfPropInfo, underlyingType);
                    }

                    else
                    {
                        result = Convert.ChangeType(row[column], propertyType);
                    }

                    property.SetValue(item, result, null);
                    index++;
                }
            }
            catch (Exception)
            {
                LogWriter.Write($"DataRow.ToObject<T>() Extension method - Failed to parse {row} to Type {typeOfT}. ArgumentException thrown.");
                throw new ArgumentException($"Failed to parse {row} to {typeOfT}.");
            }
            return item;
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        // needs unit tests
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void AddMany<T>(this ObservableCollection<T> list, ObservableCollection<T> elements)
        {
            foreach (T item in elements)
            {
                list.Add(item);
            }
        }
    }
}
