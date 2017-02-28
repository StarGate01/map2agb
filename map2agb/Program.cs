using map2agblib.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Data;

namespace map2agb
{

    class Program
    {

        static void PrintUsage()
        {
            Console.WriteLine("Usage: " + Environment.NewLine +
                "map2agb.exe <project path>");
        }

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsage();
                return 1;
            }

            string projectPath = args[1];
            RomData data = null;
            try
            {
                data = RomData.ImportFromDirectory(projectPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error opening project: " + ex.Message);
                return 1;
            }
            Console.WriteLine("Opened project");

            //Do work with data

            return 0;
        }

    }

}
