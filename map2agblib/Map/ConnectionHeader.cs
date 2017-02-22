using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{
    public class ConnectionHeader
    {
        #region Properties
        /// <summary>
        /// Gets the List of Maps stored in this ConnectionHeader
        /// </summary>
        public List<Connection> Connections { get; private set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ConnectionHeader object with initial connection capacity of count
        /// </summary>
        public ConnectionHeader()
        {
            Connections = new List<Connection>();
        }
        #endregion
    }
}
