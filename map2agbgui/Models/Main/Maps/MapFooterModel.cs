using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib.Map;
using map2agblib.Tilesets;
using map2agblib.Data;
using System.Collections.ObjectModel;
using map2agbgui.Models.BlockEditor;

namespace map2agbgui.Models.Main.Maps
{

    public class MapFooterModel
    {

        #region Properties

        #region Meta properties

        private MainModel _mainModel;
        public MainModel MainModel
        {
            get
            {
                return _mainModel;
            }
        }

        #endregion

        #region Data properties

        private string _firstTilesetID, _secondTilesetID;
        public string FirstTilesetID
        {
            get
            {
                return _firstTilesetID;
            }
            set
            {
                _firstTilesetID = value;
                RaisePropertyChanged("FirstTilesetID");
                RaisePropertyChanged("FirstTileset");
            }
        }
        public string SecondTilesetID
        {
            get
            {
                return _secondTilesetID;
            }
            set
            {
                _secondTilesetID = value;
                RaisePropertyChanged("SecondTilesetID");
                RaisePropertyChanged("SecondTileset");
            }
        }

        public TilesetModel FirstTileset
        {
            get
            {
                return MainModel.BlockEditorViewModel.Tilesets.First(p => p.Index == _firstTilesetID).Value;
            }
        }
        public TilesetModel SecondTileset
        {
            get
            {
                return MainModel.BlockEditorViewModel.Tilesets.First(p => p.Index == _secondTilesetID).Value;
            }
        }

        private ObservableCollection<ObservableCollection<ushort>> _borderBlock, _mapBlock;
        public ObservableCollection<ObservableCollection<ushort>> BorderBlock
        {
            get
            {
                return _borderBlock;
            }
            set
            {
                _borderBlock = value;
                RaisePropertyChanged("BorderBlock");
            }
        }
        public ObservableCollection<ObservableCollection<ushort>> MapBlock
        {
            get
            {
                return _mapBlock;
            }
            set
            {
                _mapBlock = value;
                RaisePropertyChanged("MapBlock");
            }
        }

        private uint _height, _width;
        public uint Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }
        public uint Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }

        private byte _borderWidth, _borderHeight;
        public byte BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = value;
                RaisePropertyChanged("BorderWidth");
            }
        }
        public byte BorderHeight
        {
            get
            {
                return _borderHeight;
            }
            set
            {
                _borderHeight = value;
                RaisePropertyChanged("BorderHeight");
            }
        }

        private ushort _padding;
        public ushort Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                RaisePropertyChanged("Padding");
            }
        }

        #endregion

        #endregion

        #region Constructor

        public MapFooterModel(MapFooter footer, MainModel mainModel)
        {
            _firstTilesetID = footer.FirstTilesetID;
            _secondTilesetID = footer.SecondTilesetID;
            _borderBlock = new ObservableCollection<ObservableCollection<ushort>>(footer.BorderBlock.Select(p => new ObservableCollection<ushort>(p)));
            _mapBlock = new ObservableCollection<ObservableCollection<ushort>>(footer.MapBlock.Select(p => new ObservableCollection<ushort>(p)));
            _height = footer.Height;
            _width = footer.Width;
            _borderHeight = footer.BorderHeight;
            _borderWidth = footer.BorderWidth;
            _padding = footer.Padding;
            _mainModel = mainModel;
        }

        public MapFooterModel() : this(new MapFooter(), new MainModel(MockData.MockRomData()))
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MapModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public MapFooter ToRomData()
        {
            MapFooter footer = new MapFooter();
            footer.FirstTilesetID = _firstTilesetID;
            footer.SecondTilesetID = _secondTilesetID;
            footer.MapBlock = _mapBlock.Select(p => p.ToArray()).ToArray();
            footer.BorderBlock = _borderBlock.Select(p => p.ToArray()).ToArray();
            footer.Height = _height;
            footer.Width = _width;
            footer.BorderHeight = _borderHeight;
            footer.BorderWidth = _borderWidth;
            footer.Padding = _padding;
            return footer;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MapFooterModel GetCopy()
        {
            MapFooterModel copy = (MapFooterModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
