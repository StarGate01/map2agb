using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{
    public class ConnectionHeader
    {

        #region Enums
        public enum ConnectionDirection { None, South, North, West, East, Dive, ReturnFromDive };
        #endregion

        #region Structures
        public struct Connection
        {
            ConnectionDirection direction;
            uint displacement;
            byte bank, map, FieldA, FieldB;
        }
        #endregion


        #region Properties
        /// <summary>
        /// All connected Maps
        /// </summary>
        public List<Connection> Connections { get; private set; }
        #endregion

        #region Constructor
        public ConnectionHeader()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
