using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib.Data;
using System.Collections.ObjectModel;

namespace map2agbgui.Models.NSEditor
{
    public class NSEditorModel : INotifyPropertyChanged
    {

        #region Properties

        private BindingList<NameEntryModel> _names;
        public BindingList<NameEntryModel> Names
        {
            get
            {
                return _names;
            }
            set
            {
                _names = value;
                RaisePropertyChanged("Names");
            }
        }

        #endregion

        #region Constructors

        public NSEditorModel(string[] names)
        {
            _names = new BindingList<NameEntryModel>(names.Select((p, pi) => new NameEntryModel(pi, p)).ToList());
        }

        public NSEditorModel() : this((MockData.MockRomData()).NameTable.Names)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public string[] ToRomData()
        {
           return Names.Select(p => p.Name).ToArray();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NSEditorModel GetCopy()
        {
            NSEditorModel copy = (NSEditorModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }
}
