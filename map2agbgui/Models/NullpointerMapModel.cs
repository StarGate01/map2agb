using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace map2agbgui.Models
{
    public class NullpointerMapModel : IMapModel, INotifyPropertyChanged
    {

        public NullpointerMapModel() { }

        public string FormatString
        {
            get
            {
                return ToString();
            }
        }

        public MapEntryType Mode
        {
            get
            {
                return MapEntryType.Nullpointer;
            }
        }

        public Uri IconPath
        {
            get
            {
                return new Uri(@"/Assets/None_16x.png", UriKind.Relative);
            }
        }

        public override string ToString()
        {
            return "Nullpointer";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NullpointerMapModel GetCopy()
        {
            NullpointerMapModel copy = (NullpointerMapModel)this.MemberwiseClone();
            return copy;
        }

    }

}
