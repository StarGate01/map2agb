using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace map2agblib.Data
{

    /// <summary>
    /// Provides generic methods for importing and exporting data from and to xml files.
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    [DataContract]
    public abstract class XMLImportExport<T> : IImportExport<T>
    {

        private static DataContractSerializer serializer = new DataContractSerializer(typeof(T));

        /// <summary>
        /// Exports data to a xml file.
        /// </summary>
        /// <param name="data">The data to export</param>
        /// <param name="filePath">The target file path</param>
        public void ExportToFile(T data, string filePath)
        {
            using (FileStream output = File.Open(filePath, FileMode.Create, FileAccess.Write))
            {
                using (XmlWriter writer = XmlWriter.Create(output, new XmlWriterSettings() { Indent = true }))
                {
                    serializer.WriteObject(writer, data);
                    writer.Flush();
                    writer.Close();
                }
                output.Flush();
                output.Close();
            }
        }

        /// <summary>
        /// Imports data from a xml file
        /// </summary>
        /// <param name="filePath">The target file path</param>
        /// <returns>The imported data</returns>
        public T ImportFromFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            T data;
            using (FileStream input = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                data = (T)serializer.ReadObject(input);
                input.Close();
            }
            return data;
        }

    }

}
