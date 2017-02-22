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
        /// <param name="count">Initial capacity of the Connections List</param>
        public ConnectionHeader(int count)
        {
            Connections = new List<Connection>(count);
        }
        #endregion
    }
}
