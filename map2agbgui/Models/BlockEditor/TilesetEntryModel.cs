using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using map2agblib.Tilesets;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetEntryModel : IRomSerializable<TilesetEntryModel, TilesetEntry>, INotifyPropertyChanged
    {

        #region Properties

        private BlockBehaviourModel _behaviour;
        public BlockBehaviourModel Behaviour
        {
            get
            {
                return _behaviour;
            }
            set
            {
                _behaviour = value;
                RaisePropertyChanged("Behaviour");
            }
        }

        private BlockTilemapModel _tilemap;
        public BlockTilemapModel Tilemap
        {
            get
            {
                return _tilemap;
            }
            set
            {
                _tilemap = value;
                RaisePropertyChanged("Tilemap");
            }
        }

        #endregion

        #region Constructor

        public TilesetEntryModel(TilesetEntry entry) : base(entry)
        {
            _behaviour = new BlockBehaviourModel(entry.Behaviour);
            _tilemap = new BlockTilemapModel(entry.TilemapEntry);
        }

        #endregion

        #region Methods

        public override TilesetEntry ToRomData()
        {
            TilesetEntry data = new TilesetEntry();
            data.Behaviour = _behaviour.ToRomData();
            data.TilemapEntry = _tilemap.ToRomData();
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
