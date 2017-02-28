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
    public class MapHeaderModel : IMapModel, INotifyPropertyChanged
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

        public string Name
        {
            get
            {
                return _mainModel.NSEditorViewModel.Names[_nameID].Name;
            }
        }

        private int _nameID;
        public int NameID
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

        #endregion

        #endregion

        #region Constructor

        public MapHeaderModel(BankModel bank, MapHeader header, MainModel mainModel)
        {
            _bank = bank;
            _nameID = header.Name;
            _footer = new MapFooterModel(header.Footer, mainModel);
            _mainModel = mainModel;
        }

        public MapHeaderModel() : this(null, new MapHeader() { Name = 0 }, new MainModel(MockData.MockRomData()))
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

        public MapHeader ToRomData()
        {
            MapHeader header = new MapHeader();
            header.Name = (byte)NameID;
            header.Footer = Footer.ToRomData();
            return header;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MapHeaderModel GetCopy()
        {
            MapHeaderModel copy = (MapHeaderModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
