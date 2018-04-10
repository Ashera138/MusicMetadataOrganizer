using System;
using System.Text.RegularExpressions;

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
    }
}
