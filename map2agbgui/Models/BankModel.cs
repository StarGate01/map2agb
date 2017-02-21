using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace map2agbgui.Models
{

    public class BankModel : INotifyPropertyChanged, IFormatable
    {

        private List<NumericDisplayTuple<MapModel>> _maps;
        public List<NumericDisplayTuple<MapModel>> Maps
        {
            get
            {
                return _maps;
            }
            set
            {
                _maps = value;
                RaisePropertyChanged("Maps");
            }
        }
        public string FormatString
        {
            get
            {
                return "Bank {0}";
            }
        }

        public BankModel(): this(new List<NumericDisplayTuple<MapModel>>()) { }
        public BankModel(List<NumericDisplayTuple<MapModel>> maps)
        {
            Maps = maps;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public BankModel GetCopy()
        {
            BankModel copy = (BankModel)this.MemberwiseClone();
            return copy;
        }

    }

}
