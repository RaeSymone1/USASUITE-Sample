using System.Linq;

namespace OPM.SFS.Web.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string ValidSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }
    }
}
