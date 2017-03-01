using map2agblib.Tilesets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using map2agblib.Data;
using System.Collections.ObjectModel;
using map2agbgui.Models.Main;
using map2agblib.Imaging;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetModel : ITupleFormattable, INotifyPropertyChanged
    {

        #region Properties

        #region Meta properties

        public string FormatString
        {
            get
            {
                return "{0}";
            }
        }
        
        #endregion

        #region Data properties

        private LazyReference<ImageContainer> _graphic;
        public LazyReference<ImageContainer> GraphicReference
        {
            get
            {
                return _graphic;
            }
            set
            {
                _graphic = value;
                RaisePropertyChanged("GraphicReference");
                RaisePropertyChanged("Graphic");
            }
        }
        public Image Graphic
        {
            get
            {
                return _graphic.Data.Image;
            }
        }

        private bool _compressed, _secondary;
        public bool Compressed
        {
            get
            {
                return _compressed;
            }
            set
            {
                _compressed = value;
                RaisePropertyChanged("Compressed");
            }
        }
        public bool Secondary
        {
            get
            {
                return _secondary;
            }
            set
            {
                _secondary = value;
                RaisePropertyChanged("Secondary");
            }
        }

        private byte _field2, _field3;
        public byte Field2
        {
            get
            {
                return _field2;
            }
            set
            {
                _field2 = value;
                RaisePropertyChanged("Field2");
            }
        }
        public byte Field3
        {
            get
            {
                return _field3;
            }
            set
            {
                _field3 = value;
                RaisePropertyChanged("Field3");
            }
        }

        private ObservableCollection<DisplayTuple<int, PaletteModel>> _palettes;
        public ObservableCollection<DisplayTuple<int, PaletteModel>> Palettes
        {
            get
            {
                return _palettes;
            }
            set
            {
                _palettes = value;
                RaisePropertyChanged("Palettes");
            }
        }

        private ObservableCollection<TilesetEntryModel> _blocks;
        public ObservableCollection<TilesetEntryModel> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                RaisePropertyChanged("Blocks");
            }
        }

        #endregion

        #endregion

        #region Constructor

        public TilesetModel(LazyReference<Tileset> tileset)
        {
            _graphic = new LazyReference<ImageContainer>(tileset.Data.Graphic);
            _compressed = tileset.Data.Compressed;
            _secondary = tileset.Data.Secondary;
            _field2 = tileset.Data.Field2;
            _field3 = tileset.Data.Field3;
            _palettes = new ObservableCollection<DisplayTuple<int, PaletteModel>>(tileset.Data.Palettes.Select((p, pi) => new DisplayTuple<int, PaletteModel>(pi, new PaletteModel(p))));
            _blocks = new ObservableCollection<TilesetEntryModel>(tileset.Data.Blocks.Select(p => new TilesetEntryModel(p)));
        }

        #endregion

        #region Methods

        public Tileset ToRomData()
        {
            Tileset tileset = new Tileset();
            tileset.Graphic = _graphic.AbsolutePath;
            tileset.Compressed = _compressed;
            tileset.Secondary = _secondary;
            tileset.Field2 = _field2;
            tileset.Field3 = _field3;
            tileset.Palettes = _palettes.Select(p => p.Value.ToRomData()).ToArray();
            tileset.Blocks = _blocks.Select(p => p.ToRomData()).ToArray();
            return tileset;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TilesetModel GetCopy()
        {
            TilesetModel copy = (TilesetModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
