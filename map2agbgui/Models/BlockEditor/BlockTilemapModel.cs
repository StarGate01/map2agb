using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Tilesets;
using System.Windows.Media.Media3D;
using map2agbgui.Extensions;

namespace map2agbgui.Models.BlockEditor
{

    public class BlockTilemapModel : IRomSerializable<BlockTilemapModel, BlockTilemap>, IRaisePropertyChanged
    {

        #region Properties

        private ushort _tileID;
        [PropertyDependency("ShaderParams")]
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
        [PropertyDependency("ShaderParams")]
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
        [PropertyDependency("ShaderParams")]
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
        [PropertyDependency("ShaderParams")]
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

        public Point4D ShaderParams
        {
            get
            {
                return new Point4D(_hFlip? 1:0, _vFlip? 1:0, (((float)_palIndex) / 16f) + (1f / 32f), (float)_tileID);
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
