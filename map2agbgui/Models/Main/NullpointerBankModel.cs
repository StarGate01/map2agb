using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace map2agbgui.Models.Main
{

    public class NullpointerBankModel : IBankModel, INotifyPropertyChanged
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

        public NullpointerBankModel()
        {
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Bank space";
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NullpointerBankModel GetCopy()
        {
            NullpointerBankModel copy = (NullpointerBankModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
