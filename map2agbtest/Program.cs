using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Imaging.JASCPAL;
using System.Drawing;

namespace map2agbtest
{

    class Program
    {

        static void Main(string[] args)
        {

            Color[] palette = JASCPALImport.Import(@"C:\Users\Christoph\Desktop\deko-iv.pal");

            Console.ReadKey();

        }

    }

}
