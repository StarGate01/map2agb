using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using map2agblib.Data;
using System.ComponentModel;

namespace map2agbgui.Models.NSEditor
{

    public class NameEntryModel : INotifyPropertyChanged
    {

        #region Properties

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value.Length <= MapNameTable.MAX_CHARS) _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private byte _index;
        public byte Index
        {
            get
            {
                return _index;
            }
        }

        public string DisplayIndex
        {
            get
            {
                return "0x" + _index.ToString("X2"); ;
            }
        }

        #endregion

        #region Constructors

        public NameEntryModel(byte index, string name)
        {
            _index = index;
            _name = name;
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
