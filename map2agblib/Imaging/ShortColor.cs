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

        public ShortColor()
        {
        }

        public ShortColor(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public ShortColor(ushort color)
        {
            Red = (byte)(color & 0x1F);
            Green = (byte)((color & 0x3E0) >> 5);
            Blue = (byte)((color & 0x7C00) >> 10);
        }

    }

}
