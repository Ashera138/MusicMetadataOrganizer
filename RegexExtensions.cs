using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicMetadataUpdater_v2._0
{
    internal static class RegexExtensions
    {
        internal static string Replace(this Group group, string source, string replacement)
        {
            return source.Substring(0, group.Index) + replacement + source.Substring(group.Index + group.Length);
        }
    }
}
