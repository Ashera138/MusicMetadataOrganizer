using System;

namespace MusicMetadataUpdater_v2._0
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string string1, string string2)
        {
            return string1.Equals(string2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
