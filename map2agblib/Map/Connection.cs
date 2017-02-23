using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{

    /// <summary>
    /// Represents a Connection to another MapHeader object and specifies its attributes
    /// </summary>
    public class Connection
    {

        /// <summary>
        /// Defines a Direction which is used to determine the side on which the connected map has to be rendered to
        /// </summary>
        public enum ConnectionDirection { None, South, North, West, East, Dive, ReturnFromDive };

        /// <summary>
        /// Gets or sets the Direction
        /// </summary>
        public ConnectionDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the displacement, the map is rendered Displacement Tiles up/down; left/right from the map, depending on the Direction it is attached and the Sign of Displacement
        /// </summary>
        public int Displacement { get; set; }

        /// <summary>
        /// Gets or sets the Bank of the Map connected
        /// </summary>
        byte Bank { get; set; }

        /// <summary>
        /// Gets or sets the Number of the Map connected
        /// </summary>
        byte Map { get; set; }

        /// <summary>
        /// Gets or sets FieldA, possibly a Padding
        /// </summary>
        byte FieldA { get; set; }

        /// <summary>
        /// Gets or sets FieldB, possibly a Padding
        /// </summary>
        byte FieldB { get; set; }

        /// <summary>
        /// Creates a new skeleton connection
        /// </summary>
        public Connection()
        {

        }
    }
}
