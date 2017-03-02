using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib.Map;
using System.Collections.ObjectModel;
using map2agblib.Data;
using map2agbgui.Models.Main.Maps;
using map2agbgui.Extensions;

namespace map2agbgui.Models.Main
{

    public class BankModel : IRomSerializable<BankModel, List<LazyReference<MapHeader>>>, INotifyPropertyChanged, IBankModel
    {

        #region Properties

        public bool IsSelected { get; set; } = false;

        private ObservableCollectionEx<DisplayTuple<int, IMapModel>> _maps;
        public ObservableCollectionEx<DisplayTuple<int, IMapModel>> Maps
        {
            get
            {
                return _maps;
            }
            set
            {
                _maps = value;
                _maps.ItemPropertyChanged += Maps_ItemPropertyChanged;
                _maps.CollectionChanged += Maps_CollectionChanged;
                RaisePropertyChanged("Maps");
            }
        }

        public string FormatString
        {
            get
            {
                return "{0} {1}";
            }
        }

        public BankEntryType EntryMode
        {
            get
            {
                return BankEntryType.Bank;
            }
        }

        public bool Valid
        {
            get
            {
                return _maps.All(p => p.Value.EntryMode == MapEntryType.Nullpointer || ((MapHeaderModel)p.Value).Valid);
            }
        }

        #endregion

        #region Constructor

        public BankModel(List<LazyReference<MapHeader>> headers, MainModel mainModel) : base(headers)
        {
            _maps = new ObservableCollectionEx<DisplayTuple<int, IMapModel>>(headers.Select((p, pi) => 
                new DisplayTuple<int, IMapModel>(pi, (p == null) ? (IMapModel)(new NullpointerMapModel(this)) : new MapHeaderModel(p, this, mainModel))));
            _maps.ItemPropertyChanged += Maps_ItemPropertyChanged;
            _maps.CollectionChanged += Maps_CollectionChanged;
        }

        #endregion

        #region Events

        private void Maps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("Valid");
        }

        private void Maps_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value.Valid") RaisePropertyChanged("Valid");
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Bank";
        }

        public override List<LazyReference<MapHeader>> ToRomData()
        {
            List<LazyReference<MapHeader>> headers = new List<LazyReference<MapHeader>>();
            headers = Maps.Select(p => (p.Value.EntryMode == MapEntryType.Nullpointer) ? null : ((MapHeaderModel)p.Value).ToRomData()).ToList();
            return headers;
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
