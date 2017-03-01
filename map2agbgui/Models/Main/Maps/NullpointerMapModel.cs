using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Map;

namespace map2agbgui.Models.Main.Maps
{
    public class NullpointerMapModel : IRomSerializable<MapHeaderModel, MapHeader>, IMapModel, INotifyPropertyChanged
    {

        #region Properties

        public bool IsSelected { get; set; }

        private BankModel _bank;
        public BankModel Bank
        {
            get
            {
                return _bank;
            }
        }

        public string FormatString
        {
            get
            {
                return "{0} {1}";
            }
        }

        public MapEntryType EntryMode
        {
            get
            {
                return MapEntryType.Nullpointer;
            }
        }

        #endregion

        #region Constructors

        public NullpointerMapModel(BankModel bank) : base(null)
        {
            _bank = bank;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Map space";
        }

        public override MapHeader ToRomData()
        {
            return null;
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
