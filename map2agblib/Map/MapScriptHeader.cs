using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{
    public class MapScriptHeader
    {

        #region Enums
        public enum MapScriptTypes { None, OnBlockDeltaSync, AfterStepOrEnter, BeforeSetup, AfterEventSync, Type5, Type6, Type7};
        #endregion

        #region Structures
        public struct MapScript
        {
            public MapScriptTypes Type;
            public string MapScriptData; //Todo - interface for different MapScript Data Structures
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of Scripts on the Event Page
        /// </summary>
        public List<MapScript> MapScripts { get; private set; }
        #endregion

        #region Constructor
        public MapScriptHeader()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
