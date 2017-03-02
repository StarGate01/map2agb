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
    public class MapHeaderModel : IRomSerializable<MapHeaderModel, LazyReference<MapHeader>>, IMapModel, INotifyPropertyChanged
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

        public bool Valid
        {
            get
            {
                return SettingsValid;
            }
        }
        public bool SettingsValid
        {
            get
            {
                return Footer.ValidTileSets;
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
                _footer.PropertyChanged += Footer_PropertyChanged;
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

        public MapHeaderModel(LazyReference<MapHeader> header, BankModel bank, MainModel mainModel) : base(header)
        {
            _bank = bank;
            _nameID = header.Data.Name;
            _footer = new MapFooterModel(header.Data.Footer, mainModel);
            _footer.PropertyChanged += Footer_PropertyChanged;
            _music = header.Data.Music;
            _index = header.Data.Index;
            _unknown = header.Data.Unknown;
            _flash = header.Data.Flash;
            _weather = header.Data.Weather;
            _type = header.Data.Type;
            _showName = header.Data.ShowName;
            _battleStyle = header.Data.BattleStyle;
            _mainModel = mainModel;
        }

#if DEBUG
        public MapHeaderModel() : this(new LazyReference<MapHeader>(new MapHeader() { Name = 0, Footer = new MapFooter() { FirstTilesetID = "TSE0", SecondTilesetID = "TSE245157" } }),
            null,
            new MainModel(MockData.MockRomData()))
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MapModel can only be constructed without parameters by the designer");
        }
#endif

    #endregion

    #region Events

        private void Footer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ValidTileSets")
            {
                RaisePropertyChanged("SettingsValid");
                RaisePropertyChanged("Valid");
            }
        }

#endregion

#region Methods

        public override string ToString()
        {
            return Name;
        }

        public override LazyReference<MapHeader> ToRomData()
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
            return new LazyReference<MapHeader>(header);
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
