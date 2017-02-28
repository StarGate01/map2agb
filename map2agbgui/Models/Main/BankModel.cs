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

namespace map2agbgui.Models.Main
{

    public class BankModel : INotifyPropertyChanged, ITupleFormattable
    {

        #region Properties

        private BindingList<NumericDisplayTuple<IMapModel>> _maps;
        public BindingList<NumericDisplayTuple<IMapModel>> Maps
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

        #endregion

        #region Constructor

        public BankModel(List<LazyReference<MapHeader>> headers, MainModel mainModel)
        {
            _maps = new BindingList<NumericDisplayTuple<IMapModel>>(headers.Select((p, pi) => 
                new NumericDisplayTuple<IMapModel>(pi, (p == null) ? (IMapModel)(new NullpointerMapModel(this)) : new MapModel(this, p.Data, mainModel))).ToList());
        }

        #endregion

        #region Methods

        public List<LazyReference<MapHeader>> ToRomData()
        {
            List<LazyReference<MapHeader>> headers = new List<LazyReference<MapHeader>>();
            headers = Maps.Select(p => (p.Value.EntryMode == MapEntryType.Nullpointer) ? null : new LazyReference<MapHeader>(((MapModel)p.Value).ToRomData())).ToList();
            return headers;
        }

        #endregion

        #region INotifyPropertyChanged

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

        #endregion

    }

}
