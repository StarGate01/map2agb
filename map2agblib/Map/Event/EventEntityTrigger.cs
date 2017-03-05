using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map.Event
{
    public class EventEntityTrigger : IEventEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Height of the Trigger (That is, the "Level", used for collision detection)
        /// </summary>
        public byte Height { get; set; }

        /// <summary>
        /// Gets or sets the X Coordinate of the Trigger
        /// </summary>
        public short X { get; set; }

        /// <summary>
        /// Gets or sets the Y Coordinate of the Trigger
        /// </summary>
        public short Y { get; set; }

        /// <summary>
        /// Gets or sets Field5, probably a Padding
        /// </summary>
        public byte Field5 { get; set; }

        /// <summary>
        /// Gets or sets the Variable of the Trigger
        /// </summary>
        public ushort Variable { get; set; }

        /// <summary>
        /// Gets or sets the Value of the Trigger
        /// </summary>
        public ushort Value { get; set; }

        /// <summary>
        /// Gets or sets FieldA, probably a Padding
        /// </summary>
        public byte FieldA { get; set; }

        /// <summary>
        /// Gets or sets FieldB, probably a Padding
        /// </summary>
        public byte FieldB { get; set; }

        /// <summary>
        /// Gets or sets the Script, it is only executed if Variable != Value - (???) - Variable < Value
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the InternalScript of the trigger, used when importing from ROM
        /// </summary>
        public uint InternalScript { get; set; }
        #endregion

        #region Constructor
        public EventEntityTrigger()
        {
        }
        #endregion
    }
}
