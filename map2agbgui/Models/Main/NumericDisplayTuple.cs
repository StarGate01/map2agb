using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace map2agbgui.Models.Main
{
    public class NumericDisplayTuple<T> : INotifyPropertyChanged where T : ITupleFormattable
    {

        #region Properties

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
                RaisePropertyChanged("DisplayValue");
            }
        }

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
                RaisePropertyChanged("DisplayValue");
            }
        }

        public string DisplayValue
        {
            get
            {
                return String.Format(Value.FormatString, Index.ToString(), Value.ToString());
            }
        }

        #endregion

        #region Constructors

        public NumericDisplayTuple(int index, T value)
        {
            Index = index;
            Value = value;
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
