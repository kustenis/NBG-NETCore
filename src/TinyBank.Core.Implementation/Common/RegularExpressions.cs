using System.Text.RegularExpressions;

namespace TinyBank.Core.Implementation.Common
{
    public static class RegularExpressions
    {
        public static Regex DigitOnlyRegex = new Regex(@"[^\d]");
    }
}
