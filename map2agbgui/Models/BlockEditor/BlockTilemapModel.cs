using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Tilesets;
using System.Windows.Media.Media3D;
using map2agbgui.Extensions;
using System.Windows.Media;

namespace map2agbgui.Models.BlockEditor
{

    public class BlockTilemapModel : IRomSerializable<BlockTilemapModel, BlockTilemap>, IRaisePropertyChanged
    {

        #region Properties

        private ushort _tileID;
        public ushort TileID
        {
            get
            {
                return _tileID;
            }
            set
            {
                _tileID = value;
                RaisePropertyChanged("TileID");
            }
        }

        private byte _palIndex;
        public byte PalIndex
        {
            get
            {
                return _palIndex;
            }
            set
            {
                _palIndex = value;
                RaisePropertyChanged("PalIndex");
            }
        }

        private bool _hFlip, _vFlip;
        [PropertyDependency("FlipTransform")]
        public bool HFlip
        {
            get
            {
                return _hFlip;
            }
            set
            {
                _hFlip = value;
                RaisePropertyChanged("HFlip");
            }
        }
        [PropertyDependency("FlipTransform")]
        public bool VFlip
        {
            get
            {
                return _vFlip;
            }
            set
            {
                _vFlip = value;
                RaisePropertyChanged("VFlip");
            }
        }

        public ScaleTransform FlipTransform
        {
            get
            {
                return new ScaleTransform(_hFlip ? -1 : 1, _vFlip ? -1 : 1);
            }
        }

        #endregion

        #region Constructor

        private PropertyDependencyHandler _phHandler;
        public BlockTilemapModel(BlockTilemap entry) : base(entry)
        {
            _tileID = entry.TileId;
            _palIndex = entry.PalIndex;
            _hFlip = entry.HFlip;
            _vFlip = entry.VFlip;
            _phHandler = new PropertyDependencyHandler(this);
        }

        #endregion

        #region Methods

        public override BlockTilemap ToRomData()
        {
            BlockTilemap data = new BlockTilemap();
            data.TileId = _tileID;
            data.PalIndex = _palIndex;
            data.HFlip = _hFlip;
            data.VFlip = _vFlip;
            return data;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
