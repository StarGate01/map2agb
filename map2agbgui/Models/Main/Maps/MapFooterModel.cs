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
                return MainModel.Tilesets.First(p => p.Index == _firstTilesetID).Value;
            }
        }
        public TilesetModel SecondTileset
        {
            get
            {
                return MainModel.Tilesets.First(p => p.Index == _secondTilesetID).Value;
            }
        }

        #endregion

        #endregion

        #region Constructor

        public MapFooterModel(MapFooter footer, MainModel mainModel)
        {
            _firstTilesetID = footer.FirstTilesetID;
            _secondTilesetID = footer.SecondTilesetID;
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
