using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{
    public class EventHeader
    {
        #region Structures
        public struct Person
        {
            public byte Id, Picture, Field2, Field3;
            public short X, Y;
            public byte Height, Behaviour, Movement, IsTrainer, FieldD, AlertRadius;
            public String Script;
            public ushort Flag;
            public byte Field16, Field17;

        }

        public struct Warp
        {
            public short X, Y;
            public byte Height, TargetWarp, TargetMap, TargetBank;
        }

        public struct Trigger
        {
            short X, Y;
            byte Height, Field5;
            ushort Var, Value;
            byte FieldA, FieldB;
            String Script;
        }

        public struct Sign
        {
            short X, Y;
            byte Height, Type;
            byte[] Data; //Usage depends on type, might be flag or script or whatever
        }
        #endregion


        #region Properties
        //NOTE: To read the necessary PersonCount, WarpCount etc. use List.Count

        /// <summary>
        /// Gets the list of NPCs on the Event Page
        /// </summary>
        public List<Person> Persons { get; private set; }

        /// <summary>
        /// Gets the list of Warps on the Event Page
        /// </summary>
        public List<Warp> Warps { get; private set; }

        /// <summary>
        /// Gets the list of Scripts on the Event Page
        /// </summary>
        public List<Trigger> ScriptTriggers { get; private set; }

        /// <summary>
        /// Gets the list of Signs on the Event Page
        /// </summary>
        public List<Sign> Signs { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EventHeader class TODO: Implement
        /// </summary>
        public EventHeader()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
