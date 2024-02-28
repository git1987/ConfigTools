using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal static class ExtendClass
    {
        public static int ToInt(this string str, int defaultValue = 0)
        {
            if (int.TryParse(str, out var result))
            {
                return result;
            }
            return defaultValue;
        }
        public static int? ToIntOrNull(this string str)
        {
            try
            {
                int i = int.Parse(str);
                return i;
            }
            catch
            {
                return null;
            }
        }
    }
}
