using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Tilesets
{
    public class BlockBehaviour
    {
        #region Properties

        /// <summary>
        /// Represents the behavior of a block (range 0-255)
        /// </summary>
        public byte Behavior { get; set; }
    
        /// <summary>
        /// Represents the HM Usage of the block as bitfield (5 bits)
        /// </summary>
        public byte HmUsage { get; set; }

        /// <summary>
        /// Range: 4 bits
        /// </summary>
        public byte Field2 { get; set; }

        /// <summary>
        /// Range: 6 bits
        /// </summary>
        public byte Field3 { get; set; }

        /// <summary>
        /// Range: 3 bits
        /// </summary>
        public byte Field4 { get; set; }

        /// <summary>
        /// Range: 2 bits
        /// </summary>
        public byte Field5 { get; set; }

        /// <summary>
        /// Range: 3 bits
        /// </summary>
        public byte Field6 { get; set; }

        /// <summary>
        /// Range: 1 bit
        /// </summary>
        public byte Field7 { get; set; }

        #endregion

        #region Constructor
        public BlockBehaviour(uint data)
        {
            //todo from data blob
        }

        public BlockBehaviour(byte Behavior, byte HmUsage, byte Field2,
            byte Field3, byte Field4, byte Field5, byte Field6,
            byte Field7)
        {
            this.Behavior = Behavior;
            this.HmUsage = HmUsage;
            this.Field2 = Field2;
            this.Field3 = Field3;
            this.Field4 = Field4;
            this.Field5 = Field5;
            this.Field6 = Field6;
            this.Field7 = Field7;
        }
        #endregion

        #region Methods
        public uint ToUint32()
        {
            //todo: make data blob from properties
            return 0;
        }
        #endregion
    }
}
