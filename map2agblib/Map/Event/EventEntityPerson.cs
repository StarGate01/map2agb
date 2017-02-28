using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map.Event
{
    /// <summary>
    /// Represents a NPC ("Person") and specifies its attributes
    /// </summary>
    public class EventEntityPerson : IEventEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Id of the NPC, it is used to address the NPC in scripts
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets or sets the Picture of the NPC, it is used to render the NPC
        /// </summary>
        public byte Picture { get; set; }

        //NOTE: This field might be documented in my IDA Database
        /// <summary>
        /// Gets or sets Field2, which's purpose is not known
        /// </summary>
        public byte Field2 { get; set; }

        /// <summary>
        /// Gets or sets Field3, which's purpose is not known (Might be padding or part of Field2)
        /// </summary>
        public byte Field3 { get; set; }

        /// <summary>
        /// Gets or sets the X Coordinate of the NPC
        /// </summary>
        public short X { get; set; }

        /// <summary>
        /// Gets or sets the Y Coordinate of the NPC
        /// </summary>
        public short Y { get; set; }

        /// <summary>
        /// Gets or sets the facing of the NPC (That is the initial facing direction)
        /// </summary>
        public short Facing { get; set; }

        /// <summary>
        /// Gets or sets the Height of the NPC (That is, the "Level", used for collision detection)
        /// </summary>
        public byte Height { get; set; }

        /// <summary>
        /// Gets or sets the Behaviour of the NPC, corresponds to a big enumeration
        /// </summary>
        public byte Behaviour { get; set; }

        /// <summary>
        /// Gets or sets the Movement of the NPC, is used to determine the area the NPC is allowed to move on (?)
        /// </summary>
        public byte Movement { get; set; }

        /// <summary>
        /// Gets or sets the Trainer bit of the NPC, if true, the Trainer Script is executed once the player enters its AlertRadius
        /// </summary>
        public bool IsTrainer { get; set; }

        /// <summary>
        /// Gets or sets FieldD, which's purpose is unknown
        /// </summary>
        public byte FieldD { get; set; }

        /// <summary>
        /// Gets or sets FieldB, which's purpose is unknown
        /// </summary>
        public byte FieldB { get; set; }

        /// <summary>
        /// Gets or sets the AlertRadius, only used when IsTrainer equals true
        /// </summary>
        public byte AlertRadius { get; set; }

        /* As part of the UX design we should consider having a flag that allows to disable the script entirely (or setting it null on string.empty)
           since some NPCs will never be getting a script */
        /// <summary>
        /// Gets or sets the symbolic script of the NPC
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the Flag of the NPC, if it is set in-game the NPC will be invisible
        /// </summary>
        public ushort Flag { get; set; }

        /// <summary>
        /// Gets or sets the Padding, normally 0x0000 
        /// </summary>
        public ushort Padding { get; set; }
        #endregion

        #region Constructor
        public EventEntityPerson()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
