using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Imaging
{

    [DataContract]
    public class ShortColor
    {

        [DataMember]
        public byte Red { get; set; }

        [DataMember]
        public byte Blue { get; set; }

        [DataMember]
        public byte Green { get; set; }

        [IgnoreDataMember]
        public Color Color
        {
            get
            {
                return Color.FromArgb(Red << 3, Green << 3, Blue << 3);
            }
        }

        public ShortColor()
        {
        }

        public ShortColor(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

    }

}
