using map2agblib.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace map2agb
{
    class Program
    {
        static void Main(string[] args)
        {
            /* some test code to serialize a bunch of maps */
            DataContractSerializer serializer = new DataContractSerializer(typeof(MapHeader));
            FileStream fs = new FileStream("test.xml", FileMode.Open, FileAccess.Read);
            MapHeader test = (MapHeader)serializer.ReadObject(fs);
            fs.Dispose();
            MapHeader header = new MapHeader(20,24,2,2);
            header.Weather = 5;
            header.Name = 42;
            header.Music = 0x12;
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            XmlWriter writer = XmlWriter.Create("test.xml", settings);
            serializer.WriteObject(writer, header);
            writer.Dispose();
        }
    }
}
