using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib;
using map2agblib.Map;

namespace map2agbgui.Models
{
    public class MapModel : INotifyPropertyChanged, IFormatable
    {

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
                return Convert.ToString(_mapHeader.Name);
            }
        }

        public MapModel(MapHeader mapHeader)
        {
            MapHeader = mapHeader;
        }
        public string FormatString
        {
            get
            {
                return "{1} {0}";
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
