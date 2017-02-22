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

namespace map2agbgui.Models
{
    public class MapModel : IMapModel, INotifyPropertyChanged
    {

        private RomData _dataRef;

        private MapHeader _mapHeader;
        public MapHeader MapHeader
        {
            get
            {
                return _mapHeader;
            }
            set
            {
                _mapHeader = value;
                RaisePropertyChanged("MapHeader");
            }
        }

        public string Name
        {
            get
            {
                return _dataRef.NameTable[_mapHeader.Name];
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

        public MapModel(MapHeader mapHeader, RomData dataRef)
        {
            MapHeader = mapHeader;
            _dataRef = dataRef;
        }
        public string FormatString
        {
            get
            {
                return "{1} {0}";
            }
        }

        public MapEntryType Mode
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

        public override string ToString()
        {
            return Name;
        }

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

    }

}
