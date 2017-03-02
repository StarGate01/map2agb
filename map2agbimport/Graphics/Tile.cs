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

        public byte this[int index]
        {
            get
            {
                if (index >= 32)
                    throw new ArgumentOutOfRangeException("index");
                return _indices[index];
            }

            set
            {
                if (index >= 32)
                    throw new ArgumentOutOfRangeException("index");
                if (value >= 16)
                    throw new ArgumentOutOfRangeException("value");
                _indices[index] = value;
            }
        }

        #endregion
    }
}
