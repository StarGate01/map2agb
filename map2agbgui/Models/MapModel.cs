using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib;

namespace map2agbgui.Models
{
    public class MapModel : INotifyPropertyChanged, IFormatable
    {

        private string _name;
        
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public MapModel() : this("NONAME") { }

        public MapModel(string name)
        {
            Name = name;
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
