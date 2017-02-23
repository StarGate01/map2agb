using map2agblib.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace map2agb
{
    class Program
    {
        static void Main(string[] args)
        {
            /* some test code to serialize a bunch of maps */
            XmlSerializer serializer = new XmlSerializer(typeof(MapHeader));
            MapHeader header = new MapHeader();
            header.Weather = 5;
            header.Name = 42;
            header.Music = 0x12;
            FileStream fs = null;
            try
            {
                fs = new FileStream("test.xml", FileMode.Create, FileAccess.Write);
                using (TextWriter writer = new StreamWriter(fs))
                {
                    fs = null;
                    serializer.Serialize(writer, header);
                }
            }
            //Ignore exception its only a test
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
