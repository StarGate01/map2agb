using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Map;
using map2agblib.Tilesets;
using System.Runtime.Serialization;

namespace map2agblib.Data
{

    [DataContract]
    public class RomData
    {

        /// <summary>
        /// The ROMs map name table
        /// </summary>
        [DataMember]
        public MapNameTable NameTable { get; set; }

        /// <summary>
        /// List of banks, each bank contains a list of maps
        /// </summary>
        [DataMember]
        public List<List<MapHeader>> Banks { get; set; }

        /// <summary>
        /// List of the ROMs tilesets
        /// </summary>
        [DataMember]
        public List<Tileset> Tilesets { get; set; }

        public RomData()
        {
            NameTable = new MapNameTable();
            Banks = new List<List<MapHeader>>();
            Tilesets = new List<Tileset>();
        }

    }

}
