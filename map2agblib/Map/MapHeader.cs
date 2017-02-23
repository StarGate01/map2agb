using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace map2agblib.Map
{
    /// <summary>
    /// Specifies a MapHeader in a Pokémon (GBA) game.
    /// </summary>
    [DataContract]
    public class MapHeader
    {
        #region Properties

        /// <summary>
        /// The MapFooter, containing all the data
        /// </summary>
        [DataMember]
        public MapFooter Footer { get; set; }

        /// <summary>
        /// The EventHeader, containing information about NPCs, Scripts, Signs and Warpfields
        /// </summary>
        [DataMember]
        public EventHeader Events { get; set; }

        /// <summary>
        /// The ScriptHeader, containing information about OnEnter activated Scripts
        /// </summary>
        [DataMember]
        public MapScriptHeader MapScripts { get; set; }

        /// <summary>
        /// The ConnectionHeader, containing information about the physical connection between this map and others
        /// </summary>
        [DataMember]
        public ConnectionHeader Connections { get; set; }

        /// <summary>
        /// Specifies the music played on this map
        /// </summary>
        [DataMember]
        public ushort Music { get; set; }

        /// <summary>
        /// An index used in the Map Index Table to regain the Map Header when a game is saved
        /// </summary>
        [DataMember]
        public ushort Index { get; set; }

        /// <summary>
        /// Specifies the name, corresponding to a table
        /// </summary>
        [DataMember]
        public byte Name { get; set; }

        /// <summary>
        /// Specifies wether flash can be used on this map (or must be)
        /// </summary>
        [DataMember]
        public byte Flash { get; set; }

        /// <summary>
        /// Specifies the weather effects of the map, also impacting battles
        /// </summary>
        [DataMember]
        public byte Weather { get; set; }

        /// <summary>
        /// Specifies the Type of this map
        /// </summary>
        [DataMember]
        public byte Type { get; set; }

        /// <summary>
        /// field_18
        /// </summary>
        [DataMember]
        public ushort Unknown { get; set; }

        /// <summary>
        /// Specifies wether the name of the map should be shown, and how it should be shown on entering
        /// </summary>
        [DataMember]
        public byte ShowName { get; set; }

        /// <summary>
        /// Specifies the battle animation style for the map
        /// </summary>
        [DataMember]
        public byte BattleStyle { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Mapheader with default width and height
        /// </summary>
        /// <param name="width">Width of the Mapfooter</param>
        /// <param name="height">Height of the Mapfooter</param>
        public MapHeader(uint width, uint height, byte borderWidth, byte borderHeight)
        {
            Footer = new MapFooter(width, height, borderWidth, borderHeight);
            Events = new EventHeader();
            MapScripts = new MapScriptHeader();
            Connections = new ConnectionHeader();
        }

        /// <summary>
        /// Creates a new MapHeader object with width and height of zero
        /// </summary>
        public MapHeader()
        {
            Footer = new MapFooter(0,0, 0, 0);
            Events = new EventHeader();
            MapScripts = new MapScriptHeader();
            Connections = new ConnectionHeader();
        }

        #endregion
    }
}
