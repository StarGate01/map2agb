using System;

namespace map2agblib.Tilesets
{
    public class BlockBehaviour
    {
        #region Fields

        private byte _hmUsage;
        private byte _field2;
        private byte _field3;
        private byte _field4;
        private byte _field5;
        private byte _field6;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the behavior of a block (range 0-255)
        /// </summary>
        public byte Behavior { get; set; }

        /// <summary>
        /// Represents the HM Usage of the block as bitfield (5 bits)
        /// </summary>
        public byte HmUsage
        {
            get
            {
                return _hmUsage;
            }
            set
            {
                if (value > (1 << 5))
                    throw new ArgumentOutOfRangeException("value");
                _hmUsage = value;
            }
        }

        /// <summary>
        /// Range: 4 bits
        /// </summary>
        public byte Field2
        {
            get
            {
                return _field2;
            }
            set
            {
                if (value > (1 << 4))
                    throw new ArgumentOutOfRangeException("value");
                _field2 = value;
            }
        }

        /// <summary>
        /// Range: 6 bits
        /// </summary>
        public byte Field3
        {
            get
            {
                return _field3;
            }
            set
            {
                if (value > (1 << 6))
                    throw new ArgumentOutOfRangeException("value");
                _field3 = value;
            }
        }

        /// <summary>
        /// Range: 3 bits
        /// </summary>
        public byte Field4
        {
            get
            {
                return _field4;
            }
            set
            {
                if (value > (1 << 3))
                    throw new ArgumentOutOfRangeException("value");
                _field4 = value;
            }
        }

        /// <summary>
        /// Range: 2 bits
        /// </summary>
        public byte Field5
        {
            get
            {
                return _field5;
            }
            set
            {
                if (value > (1 << 2))
                    throw new ArgumentOutOfRangeException("value");
                _field5 = value;
            }
        }

        /// <summary>
        /// Range: 3 bits
        /// </summary>
        public byte Field6
        {
            get
            {
                return _field6;
            }
            set
            {
                if (value > (1 << 3))
                    throw new ArgumentOutOfRangeException("value");
                _field6 = value;
            }
        }

        /// <summary>
        /// Range: 1 bit
        /// </summary>
        public bool Field7 { get; set; }

        #endregion

        #region Constructor
        public BlockBehaviour(uint data)
        {
            //todo from data blob
        }

        /// <summary>
        /// Creates a new BlockBehavior object with default values of 0
        /// </summary>
        public BlockBehaviour()
        {

        }

        /// <summary>
        /// Creates a new Blockbehavior object with the given values
        /// </summary>
        public BlockBehaviour(byte Behavior, byte HmUsage, byte Field2,
            byte Field3, byte Field4, byte Field5, byte Field6,
            bool Field7)
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
