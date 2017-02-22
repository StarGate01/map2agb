using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map.Event
{
    /// <summary>
    /// Represents an EventEntity, generally a Person, Warp, Trigger or Sign
    /// </summary>
    public interface IEventEntity
    {
        /// <summary>
        /// Gets or sets the X Coordinate of the Entity
        /// </summary>
        short X { get; set; }

        /// <summary>
        /// Gets or sets the Y Coordinate of the Entity
        /// </summary>
        short Y { get; set; }

        /// <summary>
        /// Gets or sets the Height of the Entity (That is, the "Level", used for collision detection)
        /// </summary>
        byte Height { get; set; }
    }
}
