using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string firstString, string secondString)
        {
            return firstString.Equals(secondString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
