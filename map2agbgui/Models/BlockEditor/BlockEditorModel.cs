using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using map2agbgui.Models.Main;
using map2agbgui.Models.Main.Maps;
using map2agblib.Tilesets;
using map2agblib.Data;
using map2agbgui.Extensions;
using System.Diagnostics;

namespace map2agbgui.Models.BlockEditor
{

    public class BlockEditorModel : IRomSerializable<BlockEditorModel, Dictionary<string, LazyReference<Tileset>>>, IRaisePropertyChanged
    {

        #region Properties

        private string _filterText;
        [PropertyDependency("FilteredTilesets")]
        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                RaisePropertyChanged("FilterText");
            }
        }

        private ObservableCollectionEx<DisplayTuple<string, TilesetModel>> _tilesets;
        [PropertyDependency (new string[] { "Valid", "PrimaryTilesets", "SecondaryTilesets", "FilteredTilesets" })]
        [CollectionPropertyDependency("Tilesets")]
        [CollectionItemPropertyDependency("Value.Secondary", new string[] { "PrimaryTilesets", "SecondaryTilesets" })]
        [CollectionItemPropertyDependency("Value.Valid", "Valid")]
        public ObservableCollectionEx<DisplayTuple<string, TilesetModel>> Tilesets
        {
            get
            {
                return _tilesets;
            }
            set
            {
                _tilesets = value;
                _tilesets.ItemPropertyChanged += Tilesets_ItemPropertyChanged;
                _tilesets.CollectionChanged += Tilesets_CollectionChanged;
                RaisePropertyChanged("Tilesets");
            }
        }
        public IEnumerable<DisplayTuple<string, TilesetModel>> FilteredTilesets
        {
            get
            {
                if (_filterText == null || _filterText == "") return _tilesets;
                return _tilesets.Where(p => p.Index.ToLower().Contains(_filterText.ToLower()));
            }
        }
        public IEnumerable<DisplayTuple<string, TilesetModel>> PrimaryTilesets
        {
            get
            {
                return _tilesets.Where(p => !p.Value.Secondary);
            }
        }
        public IEnumerable<DisplayTuple<string, TilesetModel>> SecondaryTilesets
        {
            get
            {
                return _tilesets.Where(p => p.Value.Secondary);
            }
        }

        public bool Valid
        {
            get
            {
                return _tilesets.All(p => p.Value.ValidSettings);
            }
        }

        #endregion

        #region Constructor

        private PropertyDependencyHandler _phHandler;
        public BlockEditorModel(Dictionary<string, LazyReference<Tileset>> tilesets) : base(tilesets)
        {
            _tilesets = new ObservableCollectionEx<DisplayTuple<string, TilesetModel>>(tilesets.Select(p => new DisplayTuple<string, TilesetModel>(p.Key, new TilesetModel(p.Value, this))));
            foreach (DisplayTuple<string, TilesetModel> element in _tilesets)
                element.Value.SelectedPalette = element.Value.Palettes[0];
            _phHandler = new PropertyDependencyHandler(this);
            _tilesets.ItemPropertyChanged += Tilesets_ItemPropertyChanged;
            _tilesets.CollectionChanged += Tilesets_CollectionChanged;
        }

#if DEBUG
        public BlockEditorModel() : this((MockData.MockRomData()).Tilesets)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
            Tilesets[0].Value.AdditionalDesignerTilesetID = Tilesets[2].Index;
        }
#endif

        #endregion

        #region Events

        private void Tilesets_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value.Secondary")
            {
                foreach (DisplayTuple<string, TilesetModel> entry in _tilesets)
                {
                    entry.Value.RaisePropertyChanged("AdditionalDesignerTileset");
                    entry.Value.RaisePropertyChanged("ValidBlocks");
                }
            }
        }

        private void Tilesets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (DisplayTuple<string, TilesetModel> entry in _tilesets)
            {
                entry.Value.RaisePropertyChanged("AdditionalDesignerTileset");
                entry.Value.RaisePropertyChanged("ValidBlocks");
            }
        }

        #endregion

        #region Methods

        public override Dictionary<string, LazyReference<Tileset>> ToRomData()
        {
           return _tilesets.ToDictionary(k => k.Index, p => p.Value.ToRomData());
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
