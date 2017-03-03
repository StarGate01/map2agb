using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Extensions
{

    /// <summary>
    /// ObservableCollection which triggers on PropertyChanged of elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {

        public ObservableCollectionEx() : base() { SetupCollectionChanged();  }
        public ObservableCollectionEx(IEnumerable<T> data) : base(data) { SetupCollectionChanged(); }
        public ObservableCollectionEx(List<T> data) : base(data) { SetupCollectionChanged(); }

        private void SetupCollectionChanged()
        {
            CollectionChanged += ObservableCollectionEx_CollectionChanged;
            ObservableCollectionEx_CollectionChanged(this, null);
        }

        private void ObservableCollectionEx_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach(T element in Items)
            {
                element.PropertyChanged -= EntityViewModelPropertyChanged;
                element.PropertyChanged += EntityViewModelPropertyChanged;
            }
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseItemPropertyChanged(sender, e);
        }

        public event PropertyChangedEventHandler ItemPropertyChanged;
        public void RaiseItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemPropertyChanged?.Invoke(this, e);
        }

    }

}
