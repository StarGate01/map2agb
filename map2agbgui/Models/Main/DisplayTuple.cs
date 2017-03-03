using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agbgui.Extensions;

namespace map2agbgui.Models.Main
{
    public class DisplayTuple<K, T> : IRaisePropertyChanged where T : ITupleFormattable
    {

        #region Properties

        private K _index;
        public K Index
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
        [PropertyDependency("DisplayValue")]
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

        private PropertyDependencyHandler _phHandler;
        public DisplayTuple(K index, T value)
        {
            Index = index;
            Value = value;
            value.PropertyChanged += Value_PropertyChanged;
            _phHandler = new PropertyDependencyHandler(this);
        }

        #endregion

        #region Events

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Value." + e.PropertyName);
            RaisePropertyChanged("DisplayValue");
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
