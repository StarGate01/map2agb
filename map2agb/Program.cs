using map2agblib.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.IO;
using map2agblib.Data;

namespace map2agb
{

    class Program
    {

        static void PrintUsage()
        {
            Console.WriteLine("Usage: " + Environment.NewLine +
                "map2agb.exe <project_file_path.map2agb>");
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
                data = ImportExport.ImportFromFile(projectPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error opening project file: " + ex.Message);
                return 1;
            }
            Console.WriteLine("Opened project file");

            //Do work with data

            return 0;
        }

    }

}
