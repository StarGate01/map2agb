using System;
using System.Drawing;
namespace map2agblib.Imaging
{
    public class Palette
    {
        private Color[] _colors;

        #region Properties

        /// <summary>
        /// Represents the Array of Colors that are stored 
        /// </summary>
        public Color this[int index]
        {
            get
            {
                if (index >= 16)
                    throw new IndexOutOfRangeException();
                return _colors[index];
            }
            set
            {
                if (index >= 16)
                    throw new IndexOutOfRangeException();
                _colors[index] = value;
            }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Represents a 16 color array
        /// </summary>
        public Palette()
        {
            _colors = new Color[16];
        }
        #endregion
    }
}
