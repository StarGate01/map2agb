using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib;
using System.ComponentModel;
using System.Windows;
using map2agblib.Data;
using map2agblib.Map;
using map2agbgui.Models.NSEditor;
using map2agbgui.Models.Main.Maps;
using System.Collections.ObjectModel;
using map2agblib.Tilesets;

namespace map2agbgui.Models.Main
{
    public class MainModel : INotifyPropertyChanged
    {

        #region Properties

        private NSEditorModel _NSEditorDataModel;
        public NSEditorModel NSEditorViewModel
        {
            get
            {
                return _NSEditorDataModel;
            }
            set
            {
                _NSEditorDataModel = value;
                RaisePropertyChanged("NSEditorDataModel");
            }
        }

        private ObservableCollection<DisplayTuple<int, IBankModel>> _banks;
        public ObservableCollection<DisplayTuple<int, IBankModel>> Banks
        {
            get
            {
                return _banks;
            }
            set
            {
                _banks = value;
            }
        }

        private ObservableCollection<DisplayTuple<string, TilesetModel>> _tilesets;
        public ObservableCollection<DisplayTuple<string, TilesetModel>> Tilesets
        {
            get
            {
                return _tilesets;
            }
            set
            {
                _tilesets = value;
                RaisePropertyChanged("Tilesets");
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        #endregion

        #region Constructors

        public MainModel() : this(MockData.MockRomData())
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MainModel can only be constructed without parameters by the designer");
            ((BankModel)Banks.First().Value).Maps.First().Value.IsSelected = true;
            Status = "Designer Mode";
        }

        public MainModel(RomData romData)
        {
            _NSEditorDataModel = new NSEditorModel(romData.NameTable.Names);
            _NSEditorDataModel.Names.ListChanged += NSEditor_Names_ListChanged;
            _banks = new ObservableCollection<DisplayTuple<int, IBankModel>>(romData.Banks.Select((p, pi) =>
                new DisplayTuple<int, IBankModel>(pi, (p == null)? (IBankModel)new NullpointerBankModel() : new BankModel(p, this))));
            _tilesets = new ObservableCollection<DisplayTuple<string, TilesetModel>>(romData.Tilesets.Select(p => new DisplayTuple<string, TilesetModel>(p.Key, new TilesetModel(p.Value, this))));
        }

        #endregion

        #region Events

        private void NSEditor_Names_ListChanged(object sender, ListChangedEventArgs e)
        {
            foreach(DisplayTuple<int, IBankModel> bank in _banks)
                if (bank.Value.EntryMode == BankEntryType.Bank)
                    foreach(DisplayTuple<int, IMapModel> map in ((BankModel)bank.Value).Maps)
                        if(map.Value.EntryMode == MapEntryType.Map) map.RaisePropertyChanged("DisplayValue");
        }

        #endregion

        #region Methods

        public RomData SaveToRomData()
        {
            RomData romData = new RomData();
            romData.NameTable.Names = NSEditorViewModel.ToRomData();
            romData.Banks = Banks.Select(p => (p.Value.EntryMode == BankEntryType.Bank)? ((BankModel)p.Value).ToRomData() : null).ToList();
            romData.Tilesets = Tilesets.ToDictionary(k => k.Index, p => new LazyReference<Tileset>(p.Value.ToRomData()));
            return romData;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainModel GetCopy()
        {
            MainModel copy = (MainModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
