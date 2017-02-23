using map2agblib.Map.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map
{
    public class EventHeader
    {
        #region Properties
        //NOTE: To read the necessary PersonCount, WarpCount etc. use List.Count

        /// <summary>
        /// Gets the list of NPCs on the Event Page
        /// </summary>
        public List<EventEntityPerson> Persons { get; private set; }

        /// <summary>
        /// Gets the list of Warps on the Event Page
        /// </summary>
        public List<EventEntityWarp> Warps { get; private set; }

        /// <summary>
        /// Gets the list of Scripts on the Event Page
        /// </summary>
        public List<EventEntityTrigger> ScriptTriggers { get; private set; }

        /// <summary>
        /// Gets the list of Signs on the Event Page
        /// </summary>
        public List<EventEntitySign> Signs { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EventHeader class
        /// </summary>
        public EventHeader()
        {
            Persons = new List<EventEntityPerson>();
            Warps = new List<EventEntityWarp>();
            ScriptTriggers = new List<EventEntityTrigger>();
            Signs = new List<EventEntitySign>();
        }
        #endregion
    }
}
