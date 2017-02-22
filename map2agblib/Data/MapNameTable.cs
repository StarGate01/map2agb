using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Data
{
    public class MapNameTable
    {
        private const int MAX_NAME = 0x6C;
        private const int MAX_CHARS = 16;
        private string[] _names;

        public string this[int index]
        {
            get
            {
                if (index >= MAX_NAME)
                    throw new IndexOutOfRangeException();
                return _names[index];
            }
            set
            {
                if (index >= MAX_NAME)
                    throw new IndexOutOfRangeException();
                if (value.Length > MAX_CHARS)
                    throw new InvalidOperationException();
                _names[index] = value;
            }
        }

        public MapNameTable()
        {
            _names = Enumerable.Repeat("DUMMY_NAME", MAX_NAME).ToArray();
        }
    }
}
