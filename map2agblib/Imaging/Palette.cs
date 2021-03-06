﻿using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace map2agblib.Imaging
{

    [DataContract]
    public class Palette
    {

        #region Fields

        [DataMember]
        public ShortColor[] Colors { get; set; }

        #endregion

        #region Indexed properties

        /// <summary>
        /// Represents the Array of Colors that are stored 
        /// </summary>
        [IgnoreDataMember]
        public ShortColor this[int index]
        {
            get
            {
                if (index >= 16)
                    throw new IndexOutOfRangeException();
                return Colors[index];
            }
            set
            {
                if (index >= 16)
                    throw new IndexOutOfRangeException();
                Colors[index] = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Represents a 16 color array
        /// </summary>
        public Palette()
        {
            Colors = new ShortColor[16];
            for (int i = 0; i < 16; i++) Colors[i] = new ShortColor(0, 0, 0);
        }

        public Palette(ShortColor[] colors)
        {
            Colors = colors;
        }

        #endregion
    }

}
