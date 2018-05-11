using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

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
            return source.Substring(0, group.Index) + replacement + source.Substring(group.Index + group.Length);
        }

        public static long ElementValueNull(this XElement element)
        {
            return element == null ? 0 : Convert.ToInt64(element.Value);
        }

        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        public static async Task<List<T>> ToListAsync<T>(this DataTable table) where T : class, new()
        {
            return await Task.Run(() => table.ToList<T>());
        }

        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            // Not sure if the row.ToString() prints out anything useful
                            LogWriter.Write($"DataSet.ToList() Extension method - Could not parse a row in the DataTable to an object of the supplied type. " +
                                $"Row object: {row}, Type supplied: {obj.GetType()}, Type property: {prop}.");
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<T> ToIenumerableOfObject<T>(this DataRowCollection rows) where T : new()
        {
            foreach (DataRow dataRow in rows)
            {
                yield return dataRow.ToObject<T>();
            }
        }

        public static T ToObject<T>(this DataRow row) where T : new()
        {
            T item = new T();
            Type typeOfT = item.GetType();
            var index = 0;

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property = typeOfT.GetProperty(column.ColumnName);
                Type propertyType = property.PropertyType;
                object result = null;

                if (IsNullableType(propertyType))
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
            return item;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
