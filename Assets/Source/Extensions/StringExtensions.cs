using System.Linq;

namespace Assets.Source.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks that a string contains only letters specified in the format string
        /// </summary>
        public static bool ContainsOnly(this string str, string format, bool ignoreCase = false)
        {
            string stringToCheck = (ignoreCase) ? str.ToLower() : str;
            string formatToCheck = (ignoreCase) ? format.ToLower() : format;

            foreach (char c in stringToCheck)
            {
                if (!formatToCheck.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
