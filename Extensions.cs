using System;
using System.Data.Entity;
using System.Text.RegularExpressions;
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

        /*
        public static string StringElementValueNull(this XElement element)
        {
            return element == null ? string.Empty : element.Value;
        }
        */

        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
