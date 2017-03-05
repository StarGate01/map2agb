using System;

namespace map2agblib.Map.Event
{
    public class EventEntityWarp : IEventEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the X Coordinate of the Warp
        /// </summary>
        public short X { get; set; }

        /// <summary>
        /// Gets or sets the Y Coordinate of the Warp
        /// </summary>
        public short Y { get; set; }

        /// <summary>
        /// Gets or sets the Height of the Warp (That is, the "Level", used for collision detection)
        /// </summary>
        public byte Height { get; set; }

        /// <summary>
        /// Gets or sets the TargetNumber of the Warp, i.e. which exit to teleport to
        /// </summary>
        public byte TargetWarp { get; set; }

        /// <summary>
        /// Gets or sets the TargetMap of the Warp, i.e. which Map to teleport to
        /// </summary>
        public byte TargetMap { get; set; }

        /// <summary>
        /// Gets or sets the TargetBank of the Warp, i.e. which Bank to teleport to
        /// </summary>
        public byte TargetBank { get; set; }
        #endregion

        #region Constructor
        public EventEntityWarp()
        {
        }
        #endregion
    }
}
