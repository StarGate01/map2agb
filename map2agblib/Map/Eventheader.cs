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
            //TODO: Implement
        }

        public struct Warp
        {
            //TODO: Implement
        }

        public struct Script
        {
            //TODO: Implement
        }

        public struct Sign
        {
            //TODO: Implement
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
        public List<Script> Scripts { get; private set; }

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
