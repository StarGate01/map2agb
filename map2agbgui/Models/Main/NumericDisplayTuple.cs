using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace map2agbgui.Models.Main
{
    public class NumericDisplayTuple<T> : INotifyPropertyChanged where T : ITupleFormattable, INotifyPropertyChanged
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
                _value.PropertyChanged += Value_PropertyChanged;
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
            value.PropertyChanged += Value_PropertyChanged;
        }

        #endregion

        #region Events

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Value");
            RaisePropertyChanged("DisplayValue");
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NumericDisplayTuple<T> GetCopy()
        {
            NumericDisplayTuple<T> copy = (NumericDisplayTuple<T>)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
