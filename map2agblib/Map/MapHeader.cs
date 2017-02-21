﻿using System;

namespace map2agblib.Map
{
    /// <summary>
    /// Specifies a MapHeader in a Pokémon (GBA) game.
    /// </summary>
    public class MapHeader
    {
        #region Properties

        /// <summary>
        /// The MapFooter, containing all the data
        /// </summary>
        public MapFooter Footer { get; set; }

        /// <summary>
        /// The EventHeader, containing information about NPCs, Scripts, Signs and Warpfields
        /// </summary>
        public EventHeader Events { get; set; }

        /// <summary>
        /// The ScriptHeader, containing information about OnEnter activated Scripts
        /// </summary>
        public MapScriptHeader MapScripts { get; set; }

        /// <summary>
        /// The ConnectionHeader, containing information about the physical connection between this map and others
        /// </summary>
        public ConnectionHeader Connections { get; set; }

        /// <summary>
        /// Specifies the music played on this map
        /// </summary>
        public ushort Music { get; set; }

        /// <summary>
        /// An index used in the Map Index Table to regain the Map Header when a game is saved
        /// </summary>
        public ushort Index { get; set; }

        /// <summary>
        /// Specifies the name, corresponding to a table
        /// </summary>
        public byte Name { get; set; }

        /// <summary>
        /// Specifies wether flash can be used on this map (or must be)
        /// </summary>
        public byte Flash { get; set; }

        /// <summary>
        /// Specifies the weather effects of the map, also impacting battles
        /// </summary>
        public byte Weather { get; set; }

        /// <summary>
        /// Specifies the Type of this map
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// field_18
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Specifies wether the name of the map should be shown, and how it should be shown on entering
        /// </summary>
        public byte ShowName { get; set; }

        /// <summary>
        /// Specifies the battle animation style for the map
        /// </summary>
        public byte BattleStyle { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new MapHeader object TODO: Implement
        /// </summary>
        public MapHeader()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}