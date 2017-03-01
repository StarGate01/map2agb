using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib;
using map2agblib.Map;
using map2agblib.Data;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace map2agbgui.Models.Main.Maps
{
    public class MapHeaderModel : IRomSerializable<MapHeaderModel, MapHeader>, IMapModel, INotifyPropertyChanged
    {

        #region Properties

        #region Meta properties

        public bool IsSelected { get; set; } = false;

        private BankModel _bank;
        public BankModel Bank
        {
            get
            {
                return _bank;
            }
        }

        public MapEntryType EntryMode
        {
            get
            {
                return MapEntryType.Map;
            }
        }

        public string FormatString
        {
            get
            {
                return "{0} {1}";
            }
        }

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

        private byte _nameID;
        public byte NameID
        {
            get
            {
                return _nameID;
            }
            set
            {
                _nameID = value;
                RaisePropertyChanged("NameID");
                RaisePropertyChanged("Name");
            }
        }
        public string Name
        {
            get
            {
                return _mainModel.NSEditorViewModel.Names[_nameID].Name;
            }
        }

        private MapFooterModel _footer;
        public MapFooterModel Footer
        {
            get
            {
                return _footer;
            }
            set
            {
                _footer = value;
                RaisePropertyChanged("Footer");
            }
        }

        private ushort _music, _index, _unknown;
        public ushort Music
        {
            get
            {
                return _music;
            }
            set
            {
                _music = value;
                RaisePropertyChanged("Music");
            }
        }
        public ushort Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }
        public ushort Unknown
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                RaisePropertyChanged("Unknown");
            }
        }

        private byte _flash, _weather, _type, _showName, _battleStyle;
        public byte Flash
        {
            get
            {
                return _flash;
            }
            set
            {
                _flash = value;
                RaisePropertyChanged("Flash");
            }
        }
        public byte Weather
        {
            get
            {
                return _weather;
            }
            set
            {
                _weather = value;
                RaisePropertyChanged("Weather");
            }
        }
        public byte Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }
        public byte ShowName
        {
            get
            {
                return _showName;
            }
            set
            {
                _showName = value;
                RaisePropertyChanged("ShowName");
            }
        }
        public byte BattleStyle
        {
            get
            {
                return _battleStyle;
            }
            set
            {
                _battleStyle = value;
                RaisePropertyChanged("BattleStyle");
            }
        }

        #endregion

        #endregion

        #region Constructor

        public MapHeaderModel(MapHeader header, BankModel bank, MainModel mainModel) : base(header)
        {
            _bank = bank;
            _nameID = header.Name;
            _footer = new MapFooterModel(header.Footer, mainModel);
            _music = header.Music;
            _index = header.Index;
            _unknown = header.Unknown;
            _flash = header.Flash;
            _weather = header.Weather;
            _type = header.Type;
            _showName = header.ShowName;
            _battleStyle = header.BattleStyle;
            _mainModel = mainModel;
        }

        public MapHeaderModel() : this(new MapHeader() { Name = 0, Footer = new MapFooter() { FirstTilesetID = "TSE0", SecondTilesetID = "TSE245157" } },
            null,
            new MainModel(MockData.MockRomData()))
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MapModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        public override MapHeader ToRomData()
        {
            MapHeader header = new MapHeader();
            header.Name = _nameID;
            header.Footer = _footer.ToRomData();
            header.Music = _music;
            header.Index = _index;
            header.Unknown = _unknown;
            header.Flash = _flash;
            header.Weather = _weather;
            header.Type = _type;
            header.ShowName = _showName;
            header.BattleStyle = _battleStyle;
            return header;
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
