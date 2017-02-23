using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace map2agbgui.Models.Main
{
    public class NullpointerMapModel : IMapModel, INotifyPropertyChanged
    {

        #region Properties

        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }

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
     
        public Uri IconPath
        {
            get
            {
                return new Uri(@"/Assets/None_16x.png", UriKind.Relative);
            }
        }

        #endregion

        #region Constructors

        public NullpointerMapModel(BankModel bank)
        {
            _bank = bank;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Empty";
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NullpointerMapModel GetCopy()
        {
            NullpointerMapModel copy = (NullpointerMapModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
