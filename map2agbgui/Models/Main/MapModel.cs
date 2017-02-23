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

namespace map2agbgui.Models.Main
{
    public class MapModel : IMapModel, INotifyPropertyChanged
    {

        #region Properties

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

        public Uri IconPath
        {
            get
            {
                return new Uri(@"/Assets/Document_16x.png", UriKind.Relative);
            }
        }

        public string FormatString
        {
            get
            {
                return "{0} {1}";
            }
        }

        public string Name
        {
            get
            {
                return App.MainViewModel.NSEditorViewModel.Names[_nameID].Name;
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

        #endregion

        #region Constructor

        public MapModel(BankModel bank, MapHeader header)
        {
            _bank = bank;
            _nameID = header.Name;
        }

        public MapModel()
        {
            _bank = null;
            _nameID = 0;
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
            return header;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MapModel GetCopy()
        {
            MapModel copy = (MapModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
