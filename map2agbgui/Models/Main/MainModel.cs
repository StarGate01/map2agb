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
using map2agbgui.Models.BlockEditor;
using System.Collections.Specialized;
using map2agbgui.Extensions;

namespace map2agbgui.Models.Main
{
    public class MainModel : IRomSerializable<MainModel, RomData>, IRaisePropertyChanged, IDisposable
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

        private BlockEditorModel _blockEditorModel;
        [ChildPropertyDependency(new string[] { "Valid", "PrimaryTilesets", "SecondaryTilesets" }, "Valid")]
        public BlockEditorModel BlockEditorViewModel
        {
            get
            {
                return _blockEditorModel;
            }
            set
            {
                _blockEditorModel = value;
                _blockEditorModel.PropertyChanged += BlockEditorModel_PropertyChanged;
                RaisePropertyChanged("BlockEditorViewModel");
            }
        }

        private ObservableCollectionEx<DisplayTuple<int, IBankModel>> _banks;
        [CollectionPropertyDependency("MapsValid")]
        [CollectionItemPropertyDependency("Value.Valid", "MapsValid")]
        public ObservableCollectionEx<DisplayTuple<int, IBankModel>> Banks
        {
            get
            {
                return _banks;
            }
            set
            {
                _banks = value;
                RaisePropertyChanged("Banks");
            }
        }

        [PropertyDependency("Valid")]
        public bool MapsValid
        {
            get
            {
                foreach (DisplayTuple<int, IBankModel> bank in _banks) if (bank.Value.EntryMode == BankEntryType.Bank)
                        foreach (DisplayTuple<int, IMapModel> map in ((BankModel)bank.Value).Maps)
                            if (map.Value.EntryMode == MapEntryType.Map) if (!((MapHeaderModel)map.Value).Valid) return false;
                return true;
            }
        }

        public bool Valid
        {
            get
            {
                return MapsValid && _blockEditorModel.Valid;
            }
        }

        #endregion

        #region Constructors

        private PropertyDependencyHandler _phHandler;
        public MainModel(RomData romData) : base(romData)
        {
            _NSEditorDataModel = new NSEditorModel(romData.NameTable.Names);
            _NSEditorDataModel.PropertyChanged += NSEditorDataModel_PropertyChanged;
            _blockEditorModel = new BlockEditorModel(romData.Tilesets);
            _blockEditorModel.PropertyChanged += BlockEditorModel_PropertyChanged;
            _banks = new ObservableCollectionEx<DisplayTuple<int, IBankModel>>(romData.Banks.Select((p, pi) =>
                new DisplayTuple<int, IBankModel>(pi, (p == null) ? (IBankModel)new NullpointerBankModel() : new BankModel(p, this))));
            _phHandler = new PropertyDependencyHandler(this);
        }

#if DEBUG
        public MainModel() : this(MockData.MockRomData())
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("MainModel can only be constructed without parameters by the designer");
            ((BankModel)Banks.First().Value).Maps[0].Value.IsSelected = true;
        }
#endif

        #endregion

        #region Events

        private void NSEditorDataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Names")
            { 
                foreach (DisplayTuple<int, IBankModel> bank in _banks) if (bank.Value.EntryMode == BankEntryType.Bank)
                        foreach (DisplayTuple<int, IMapModel> map in ((BankModel)bank.Value).Maps)
                            if (map.Value.EntryMode == MapEntryType.Map) ((MapHeaderModel)map.Value).RaisePropertyChanged("Name");
            }
        }

        private void BlockEditorModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "PrimaryTilesets" || e.PropertyName == "SecondaryTilesets")
            { 
                foreach (DisplayTuple<int, IBankModel> bank in _banks) if (bank.Value.EntryMode == BankEntryType.Bank)
                    foreach (DisplayTuple<int, IMapModel> map in ((BankModel)bank.Value).Maps)
                        if (map.Value.EntryMode == MapEntryType.Map) ((MapHeaderModel)map.Value).Footer.RaisePropertyChanged("ValidTilesets");
            }
        }

        #endregion

        #region Methods

        public override RomData ToRomData()
        {
            RomData romData = new RomData();
            romData.NameTable.Names = NSEditorViewModel.ToRomData();
            romData.Banks = Banks.Select(p => (p.Value.EntryMode == BankEntryType.Bank)? ((BankModel)p.Value).ToRomData() : null).ToList();
            romData.Tilesets = _blockEditorModel.ToRomData();
            return romData;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public void Dispose()
        {
            BlockEditorViewModel.Dispose();
        }

    }

}
