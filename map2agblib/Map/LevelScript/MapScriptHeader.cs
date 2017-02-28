using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map.LevelScript
{
    [DataContract]
    public class MapScriptHeader
    {

        #region Enums
        public enum MapScriptTypes { None, OnBlockDeltaSync, AfterStepOrEnter, BeforeSetup, AfterEventSync, Type5, Type6, Type7};
        #endregion

        #region Structures
        public struct MapScript
        {
            public MapScriptTypes Type;
            public string MapScriptData;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of Scripts on the Event Page
        /// </summary>
        [DataMember]
        public List<MapScript> MapScripts { get; private set; }
        #endregion

        #region Constructor
        public MapScriptHeader()
        {
           
        }
        #endregion

    }
}
