using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Models.Dialogs
{

    public class ImportDialogModel: INotifyPropertyChanged
    {

        #region Properties

        private string _ROMPath;
        public string ROMPath
        {
            get
            {
                return _ROMPath;
            }
            set
            {
                _ROMPath = value;
                RaisePropertyChanged("ROMPath");
            }
        }

        private long _offset;
        public long Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
                RaisePropertyChanged("Offset");
            }
        }

        private int _bank;
        public int Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
                RaisePropertyChanged("Bank");
            }
        }

        private int _map;
        public int Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
                RaisePropertyChanged("Map");
            }
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
