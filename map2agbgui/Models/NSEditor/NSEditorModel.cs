using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using map2agblib.Data;
using System.Collections.ObjectModel;
using map2agbgui.Extensions;

namespace map2agbgui.Models.NSEditor
{

    public class NSEditorModel : IRomSerializable<NSEditorModel, string[]>, INotifyPropertyChanged
    {

        #region Properties

        private ObservableCollectionEx<NameEntryModel> _names;
        public ObservableCollectionEx<NameEntryModel> Names
        {
            get
            {
                return _names;
            }
            set
            {
                _names = value;
                _names.CollectionChanged += Names_CollectionChanged;
                _names.ItemPropertyChanged += Names_ItemPropertyChanged;
                RaisePropertyChanged("Names");
            }
        }

        #endregion

        #region Constructors

        public NSEditorModel(string[] names) : base(names)
        {
            _names = new ObservableCollectionEx<NameEntryModel>(names.Select((p, pi) => new NameEntryModel((byte)pi, p)).ToList());
            _names.CollectionChanged += Names_CollectionChanged;
            _names.ItemPropertyChanged += Names_ItemPropertyChanged;
        }

#if DEBUG
        public NSEditorModel() : this((MockData.MockRomData()).NameTable.Names)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }
#endif

#endregion

#region Events

        private void Names_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Names");
        }


        private void Names_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("Names");
        }

#endregion

#region Methods

        public override string[] ToRomData()
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

#endregion

    }

}
