using map2agblib.Map.Event;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="personCount">Initial capacity of NPCs</param>
        /// <param name="warpCount">Initial capacity of Warps</param>
        /// <param name="triggerCount">Initial capacity of Triggers</param>
        /// <param name="signCount">Initial capacity of Signs</param>
        public EventHeader(int personCount, int warpCount, int triggerCount, int signCount)
        {
            Persons = new List<EventEntityPerson>(personCount);
            Warps = new List<EventEntityWarp>(warpCount);
            ScriptTriggers = new List<EventEntityTrigger>(triggerCount);
            Signs = new List<EventEntitySign>(signCount);
        }
        #endregion
    }
}
