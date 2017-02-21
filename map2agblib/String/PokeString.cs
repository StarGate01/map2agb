using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.String
{

    public class PokeString
    {

        public static string ConvertToString(byte[] param)
        {
            return Encoding.ASCII.GetString(param);
        }

        public static byte[] ConvertToPokeString(string param)
        {
            return Encoding.ASCII.GetBytes(param);
        }

    }

}
