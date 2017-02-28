using System;
using System.Runtime.Serialization;

namespace map2agblib.Map.LevelScript
{
    [DataContract]
    public class MapScript
    {
        #region Enums
        /// <summary>
        /// Defines Types of MapScripts that are used in-game TODO: Use external List defined in ProjectConfiguration
        /// </summary>
        public enum MapScriptTypes { None, OnBlockDeltaSync, AfterStepOrEnter, BeforeSetup, AfterEventSync, Type5, Type6, Type7 };

        /// <summary>
        /// Defines Types of MapScriptLayouts that are used to structure and read the Script in-game
        /// </summary>
        public enum MapScriptLayout { None, Script, ExtendedScript };
        #endregion

        #region Fields
        private ushort _variable;
        private ushort _value;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the Layout of the Script based on its Type TODO: Read Layout association from ProjectConfiguration
        /// </summary>
        public MapScriptLayout Layout
        {
            get
            {
                switch (Type)
                {
                    case MapScriptTypes.OnBlockDeltaSync:
                    case MapScriptTypes.BeforeSetup:
                    case MapScriptTypes.Type5:
                    case MapScriptTypes.Type6:
                    case MapScriptTypes.Type7:
                        return MapScriptLayout.Script;
                    case MapScriptTypes.AfterStepOrEnter:
                    case MapScriptTypes.AfterEventSync:
                        return MapScriptLayout.ExtendedScript;
                    case MapScriptTypes.None:
                    default:
                        return MapScriptLayout.None;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Type of the MapScript, it defines its Layout
        /// </summary>
        [DataMember]
        public MapScriptTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the Script of the MapScript, it is a symbol to reference the Script
        /// </summary>
        [DataMember]
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the Variable if the Layout is a MapScriptLayout.ExtendeScript. Throws InvalidOperationException otherwise
        /// NOTE: Even though this is, for the sake of design and serialization, always get_accessable, only type 2 and 4 (by default) are able to use this Property!
        /// </summary>
        [DataMember]
        public ushort Variable
        {
            get
            {
                return _variable;
            }
            set
            {
                if (Layout != MapScriptLayout.ExtendedScript)
                    throw new InvalidOperationException();
                _variable = value;
            }
        }

        /// <summary>
        /// Gets or sets the Value if the Layout is a MapScriptLayout.ExtendeScript. Throws InvalidOperationException otherwise
        /// NOTE: Even though this is, for the sake of design and serialization, always get_accessable, only type 2 and 4 (by default) are able to use this Property!
        /// </summary>
        public ushort Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (Layout != MapScriptLayout.ExtendedScript)
                    throw new InvalidOperationException();
                _value = value;
            }
        }
        #endregion

        #region Constructor
        public MapScript(MapScriptTypes type)
        {
            Type = type;
        }
        #endregion
    }
}
