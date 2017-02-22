﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Data
{
    /// <summary>
    /// Represents the MapNameTable to read and write names in the project
    /// </summary>
    public class MapNameTable
    {
        #region Constances
        private const int MAX_NAME = 0x6C;
        private const int MAX_CHARS = 16;
        #endregion

        #region Fields
        private string[] _names;
        #endregion

        #region Indexed Properties
        /// <summary>
        /// Gets or sets a name of the Table. Throws IndexOutOfRangeException if index is not in range. Throws InvalidOperationException if the string is too long.
        /// </summary>
        /// <param name="index">Index of the Name</param>
        /// <returns></returns>
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
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new default MapNameTable and fills it with DUMMY_NAME entries
        /// </summary>
        public MapNameTable()
        {
            _names = Enumerable.Repeat("DUMMY_NAME", MAX_NAME).ToArray();
        }
        #endregion
    }
}