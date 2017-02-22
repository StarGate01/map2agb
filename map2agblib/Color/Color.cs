using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Color
{
    /// <summary>
    /// Class to represent a GBA RBG Color
    /// </summary>
    public class Color
    {
        #region Properties
        /// <summary>
        /// Red intensity in multiples of 8
        /// </summary>
        public int Red { get; set; }

        /// <summary>
        /// Blue intensity in multiples of 8
        /// </summary>
        public int Blue { get; set; }

        /// <summary>
        /// Green intensity in multiples of 8
        /// </summary>
        public int Green { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new color instance
        /// </summary>
        /// <param name="Red">Red intensity in multiples of 8</param>
        /// <param name="Blue">Blue intensity in multiples of 8</param>
        /// <param name="Green">Green intensity in multiples of 8</param>
        public Color(int Red, int Blue, int Green)
        {
            this.Red = Red;
            this.Blue = Blue;
            this.Green = Green;
        }


        //Todo: Constructor from to RGB

        #endregion

        #region Methods
        //Todo: Method to RGB

        //Todo: Static method from RGB
        #endregion
    }
}
