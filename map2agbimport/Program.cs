using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using map2agblib.Common;
using System.IO;

namespace map2agbimport
{
    class Program
    {

        static void Main(string[] args)
        {
            Options opt = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, opt))
            {
                long mapTable = opt.MapTable.ToLong(CultureInfo.InvariantCulture);
                long nameTable = opt.NameTable.ToLong(CultureInfo.InvariantCulture);
                bool error = false;
                FileStream fs = null;
                try
                {
                    fs = new FileStream(opt.InputFile, FileMode.Open, FileAccess.Read);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        fs = null;
                        /*TODO: Start Reading the object model */
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                }
                finally
                {
                    if (fs != null)
                        fs.Dispose();
                }
                if (error)
                    return;

                /* do stuff */
            }
            Console.ReadKey();
            /* don't do stuff */
        }
    }
}
