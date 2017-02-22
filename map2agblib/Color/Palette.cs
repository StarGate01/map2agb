using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Color
{
    public class Palette
    {
        #region Properties

        /// <summary>
        /// Represents the Array of Colors that are stored 
        /// </summary>
        public Color[] Colors { get; private set; }


        #endregion

        #region Constructor
        /// <summary>
        /// Represents a 16 color array
        /// </summary>
        public Palette()
        {
            this.Colors = new Color[16];
        }
        #endregion
    }
}
