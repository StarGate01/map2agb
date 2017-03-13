using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using map2agblib.Tilesets;
using System.Windows;
using map2agbgui.Extensions;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetEntryModel : IRomSerializable<TilesetEntryModel, TilesetEntry>, INotifyPropertyChanged
    {

        #region Properties

        private TilesetModel _tilesetViewModel;
        public TilesetModel TilesetViewModel
        {
            get
            {
                return _tilesetViewModel;
            }
            set
            {
                _tilesetViewModel = value;
                RaisePropertyChanged("TilesetViewModel");
            }
        }

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

        private ObservableCollectionEx<BlockTilemapModel> _tilemap;
        public ObservableCollectionEx<BlockTilemapModel> Tilemap
        {
            get
            {
                return _tilemap;
            }
            set
            {
                _tilemap = value;
                _tilemap.ItemPropertyChanged += Tilemap_ItemPropertyChanged;
                RaisePropertyChanged("Tilemap");
            }
        }

        private bool _dirty;
        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                if(_dirty == true) RaisePropertyChanged("Graphic");
                RaisePropertyChanged("Dirty");
            }
        }

        private BitmapSource _graphic;
        public BitmapSource Graphic
        {
            get
            {
                if(_dirty) _tilesetViewModel.EnsureBlockRendererRunning();
                return _graphic;
            }
            set
            {
                _graphic = value;
                RaisePropertyChanged("Graphic");
            }
        }

        #endregion

        #region Constructor

        public TilesetEntryModel(TilesetEntry entry, TilesetModel parent) : base(entry)
        {
            _behaviour = new BlockBehaviourModel(entry.Behaviour);
            _tilemap = new ObservableCollectionEx<BlockTilemapModel>(entry.TilemapEntry.Select(p => new BlockTilemapModel(p)));
            _tilemap.ItemPropertyChanged += Tilemap_ItemPropertyChanged;
            _tilesetViewModel = parent;
            _dirty = true;
        }

#if DEBUG
        public TilesetEntryModel() : this(MockData.MockRomData().Tilesets["TSE0"].Data.Blocks[0], new TilesetModel(MockData.MockRomData().Tilesets["TSE0"], new BlockEditorModel()))
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }
#endif

        #endregion

        #region Events

        private void Tilemap_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dirty = true;
        }

        #endregion

        #region Methods

        public override TilesetEntry ToRomData()
        {
            TilesetEntry data = new TilesetEntry();
            data.Behaviour = _behaviour.ToRomData();
            data.TilemapEntry = _tilemap.Select(p => p.ToRomData()).ToArray();
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
