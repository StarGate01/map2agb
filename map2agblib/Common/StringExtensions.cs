using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Common
{
    public static class StringExtensions
    {
        public static long ToLong(this string rep, IFormatProvider provider)
        {
            rep = rep.ToLower();
            try
            {
                if (rep.StartsWith("0x"))
                    return long.Parse(rep.Substring(2), NumberStyles.HexNumber, provider);
                else
                    return long.Parse(rep, NumberStyles.Integer, provider);
            }
            catch
            {
                return 0;
            }

        }
    }
}
