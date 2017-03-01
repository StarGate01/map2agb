using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Data;
using map2agblib.Map;

namespace map2agbgui.Models.Main
{

    public class NullpointerBankModel : IRomSerializable<BankModel, List<LazyReference<MapHeader>>>, INotifyPropertyChanged, IBankModel
    {

        #region Properties

        public bool IsSelected { get; set; }

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
                return BankEntryType.Nullpointer;
            }
        }

        #endregion

        #region Constructors

        public NullpointerBankModel() :base(null)
        {
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Bank space";
        }

        public override List<LazyReference<MapHeader>> ToRomData()
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
