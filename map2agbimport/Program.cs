using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace map2agbimport
{
    class Program
    {
        static void Main(string[] args)
        {
            Options opt = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, opt))
            {
                long mapTable = -1;
                long nameTable = -1;
                Console.WriteLine(mapTable);
                /* do stuff */
            }
            Console.ReadKey();
            /* don't do stuff */
        }
    }
}
