using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using map2agblib.Data;
using System.Runtime.Serialization;
using System.Xml;

namespace map2agblib.IO
{

    public class ImportExport
    {

        public const string FILE_EXT = "map2agb";
        private static DataContractSerializer serializer = new DataContractSerializer(typeof(RomData));

        /// <summary>
        /// Imports ROM data from a stream.
        /// </summary>
        /// <param name="stream">The source stream</param>
        /// <returns>The imported ROM data</returns>
        public static RomData Import(Stream stream)
        {
            return (RomData)serializer.ReadObject(stream);
        }

        /// <summary>
        /// Imports ROM data from a file.
        /// </summary>
        /// <param name="fileName">The source file name</param>
        /// <returns>The imported ROM data</returns>
        public static RomData ImportFromFile(string fileName)
        {
            RomData data = null;
            FileStream input = File.Open(fileName, FileMode.Open, FileAccess.Read);
            data = Import(input);
            input.Close();
            input.Dispose();
            return data;
        }

        /// <summary>
        /// Exports ROM data to a stream.
        /// </summary>
        /// <param name="data">The data to export</param>
        /// <param name="output">The target stream</param>
        public static void Export(RomData data, Stream output)
        {
            XmlWriter writer = XmlWriter.Create(output);
            serializer.WriteObject(writer, data);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Exports ROM data to a file.
        /// </summary>
        /// <param name="data">The data to export</param>
        /// <param name="fileName">The target file name</param>
        public static void ExportToFile(RomData data, string fileName)
        {
            FileStream output = File.Open(fileName, FileMode.Create, FileAccess.Write);
            Export(data, output);
            output.Flush();
            output.Close();
            output.Dispose();
        }

    }

}