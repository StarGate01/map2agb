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

    public class BankModel : IRomSerializable<BankModel, List<LazyReference<MapHeader>>>, IRaisePropertyChanged, IBankModel
    {

        #region Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        private ObservableCollectionEx<DisplayTuple<int, IMapModel>> _maps;
        [CollectionPropertyDependency("Valid")]
        [CollectionItemPropertyDependency("Value.Valid", "Valid")]
        public ObservableCollectionEx<DisplayTuple<int, IMapModel>> Maps
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

        private PropertyDependencyHandler _phHandler;
        public BankModel(List<LazyReference<MapHeader>> headers, MainModel mainModel) : base(headers)
        {
            _maps = new ObservableCollectionEx<DisplayTuple<int, IMapModel>>(headers.Select((p, pi) => 
                new DisplayTuple<int, IMapModel>(pi, (p == null) ? (IMapModel)(new NullpointerMapModel(this)) : new MapHeaderModel(p, this, mainModel))));
            _phHandler = new PropertyDependencyHandler(this);
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
