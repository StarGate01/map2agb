using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using map2agblib.Imaging.JASCPAL;
using System.Drawing;
using System.IO;
using map2agbimport;
using map2agblib.Map;

namespace map2agbtest
{

    class Program
    {
        static void Main(string[] args)
        {
            BinaryReader reader = new BinaryReader(new FileStream(@"D:\onedrive\Hacking\Romhacking\Ressources\Feuerrot\Pokemon Feuerrot (D).gba", FileMode.Open, FileAccess.Read));
            MapHeader header = AgbImport.HeaderFromStream(reader, 0x3525CC, 3, 0);
            header.ExportToFile(header, "test.header");
            MapHeader importedHeader = header.ImportFromFile("test.header");



        }

    }

}
