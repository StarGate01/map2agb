using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbimport.Graphics
{
    public class Tile
    {
        #region Fields

        private byte[] _indices;

        #endregion

        #region Properties

        public byte[] Indices
        {
            get
            {
                return _indices;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index >= 64)
                    throw new ArgumentOutOfRangeException("index");

                /* TODO: Simplify */
                return index % 2 == 0 ? (byte)(_indices[index / 2] & 0xF) : (byte)((_indices[index / 2] & 0xF0) >> 4);
            }

            set
            {
                if (index >= 64)
                    throw new ArgumentOutOfRangeException("index");
                if (value >= 16)
                    throw new ArgumentOutOfRangeException("value");

                /* TODO: Simplify */
                _indices[index / 2] = index % 2 == 0 ? ((byte)((_indices[index / 2] & 0xF0) | value)) : (byte)((_indices[index / 2] & 0x0F) | value << 4);
            }
        }

        public Tile(byte[] fullArray, int index)
        {
            _indices = fullArray.Skip(index * 32).Take(32).ToArray();
        }

        #endregion
    }
}
