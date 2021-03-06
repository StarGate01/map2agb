﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Tilesets
{
    public class BlockTilemap
    {
        #region Fields
        private ushort _tileId;
        private byte _palIndex;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TileId, used to determine which 8x8 Tile will be used by the TilemapEntry.
        /// TileId is part of a bitfield and 9 bit long, hence greater values will throw an ArgumentOutOfRangeException.
        /// </summary>
        public ushort TileId
        {
            get
            {
                return _tileId;
            }
            set
            {
                if (value >= 1 << 10)
                    throw new ArgumentOutOfRangeException("value");
                _tileId = value;
            }
        }

        /// <summary>
        /// Gets or sets the PalIndex, used to determine the rendering palette of the Tile.
        /// TileId is part of a bitfield and 4 bit long, hence greater values will throw an ArgumentOutOfRangeException.
        /// </summary>
        public byte PalIndex
        {
            get
            {
                return _palIndex;
            }
            set
            {
                if (value >= (1 << 4))
                    throw new ArgumentOutOfRangeException("value");
                _palIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the HFlip bit, used to mirror the tile on rendering.
        /// </summary>
        public bool HFlip { get; set; }

        /// <summary>
        /// Gets or sets the VFlip bit, used to mirror the tile on rendering.
        /// </summary>
        public bool VFlip { get; set; }
        #endregion

        public ushort ToUShort()
        {
            return (ushort)(TileId | (HFlip ? 0x400 : 0) | (VFlip ? 0x800 : 0) | (PalIndex << 12));
        }

        /// <summary>
        /// Creates a new Tilemap part with values of 0
        /// </summary>
        public BlockTilemap()
        {

        }

        public BlockTilemap(ushort blob)
        {
            TileId = (ushort)(blob & 0x3FF);
            HFlip = (blob & 0x400) > 0;
            VFlip = (blob & 0x800) > 0;
            PalIndex = (byte)((blob & 0xF000) >> 12);
        }
    }
}
