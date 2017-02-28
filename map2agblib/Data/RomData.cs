using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Map;
using map2agblib.Tilesets;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace map2agblib.Data
{

    [DataContract]
    public class RomData
    {

        private static DataContractSerializer serializer = new DataContractSerializer(typeof(RomData));

        #region Properties

        /// <summary>
        /// The ROMs map name table
        /// </summary>
        [DataMember]
        public MapNameTable NameTable { get; set; }

        /// <summary>
        /// List of banks, each bank contains a list of maps
        /// </summary>
        [DataMember]
        public List<List<LazyReference<MapHeader>>> Banks { get; set; }

        /// <summary>
        /// List of the ROMs tilesets
        /// </summary>
        [DataMember]
        public List<LazyReference<Tileset>> Tilesets { get; set; }

        #endregion

        public RomData()
        {
            NameTable = new MapNameTable();
            Banks = new List<List<LazyReference<MapHeader>>>();
            Tilesets = new List<LazyReference<Tileset>>();
        }

        #region Import and Export

        /// <summary>
        /// Imports ROM data from a directory.
        /// </summary>
        /// <param name="dirName">The source directory name</param>
        /// <returns>The imported ROM data</returns>
        public static RomData ImportFromDirectory(string dirName)
        {
            if (!Directory.Exists(dirName)) throw new DirectoryNotFoundException(dirName);
            string projFileName = Path.Combine(dirName, "project.map2agb");
            if (!File.Exists(projFileName)) throw new FileNotFoundException(projFileName);
            string mapsDirName = Path.Combine(dirName, "maps");
            if (!Directory.Exists(mapsDirName)) throw new DirectoryNotFoundException(mapsDirName);
            string tilesetsDirName = Path.Combine(dirName, "tilesets");
            if (!Directory.Exists(tilesetsDirName)) throw new DirectoryNotFoundException(tilesetsDirName);
            RomData data = null;
            using (FileStream input = File.Open(projFileName, FileMode.Open, FileAccess.Read))
            {
                data = (RomData)serializer.ReadObject(input);
                input.Close();
            }
            return data;
        }

        /// <summary>
        /// Exports ROM data to a directory.
        /// </summary>
        /// <param name="data">The data to export</param>
        /// <param name="dirName">The target directory. Will be replaced or created.</param>
        public static void ExportToDirectory(RomData data, string dirName)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "map2agb_save_" + Guid.NewGuid().ToString("D"));
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);
            string projFileName = Path.Combine(tempDir, "project.map2agb");
            Directory.CreateDirectory(Path.Combine(tempDir, "maps"));
            Directory.CreateDirectory(Path.Combine(tempDir, "tilesets"));
            for (int i = 0; i < data.Banks.Count; i++) for (int j = 0; j < data.Banks[i].Count; j++)
            {
                if (data.Banks[i][j] == null) continue;
                data.Banks[i][j].AbsolutePath = Path.Combine((Path.Combine(dirName, "maps")), i + "_" + j + ".map");
                data.Banks[i][j].ExportToFile(Path.Combine((Path.Combine(tempDir, "maps")), i + "_" + j + ".map"));
            }
            for (int i = 0; i < data.Tilesets.Count; i++)
            {
                data.Tilesets[i].AbsolutePath = Path.Combine((Path.Combine(dirName, "tilesets")), i + ".tileset");
                data.Tilesets[i].ExportToFile(Path.Combine((Path.Combine(tempDir, "tilesets")), i + ".tileset"));
            }
            using (FileStream output = File.Open(projFileName, FileMode.Create, FileAccess.Write))
            {
                using (XmlWriter writer = XmlWriter.Create(output))
                {
                    serializer.WriteObject(writer, data);
                    writer.Flush();
                    writer.Close();
                }
                output.Flush();
                output.Close();
            }
            if (Directory.Exists(dirName)) Directory.Delete(dirName, true);
            Directory.Move(tempDir, dirName);
        }

        #endregion

    }

}
