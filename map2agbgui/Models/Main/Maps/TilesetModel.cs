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

namespace map2agbgui.Models.Main.Maps
{

    public class TilesetModel
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

        #endregion

        #endregion

        #region Constructor

        public TilesetModel(LazyReference<Tileset> tileset, MainModel mainModel)
        {
            _graphic = new LazyReference<ImageContainer>(tileset.Data.Graphic);
            _mainModel = mainModel;
        }

        public TilesetModel() : this(new LazyReference<Tileset>(), new MainModel(MockData.MockRomData()))
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MapModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public Tileset ToRomData()
        {
            Tileset tileset = new Tileset();
            tileset.Graphic = _graphic.AbsolutePath;
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
