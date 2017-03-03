﻿using System;
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
                RaisePropertyChanged("FilteredTilesets");
            }
        }

        private ObservableCollectionEx<DisplayTuple<string, TilesetModel>> _tilesets;

        [PropertyDependency(new string[] { "Valid", "PrimaryTilesets", "SecondaryTilesets", "FilteredTilesets" })]
        [CollectionPropertyDependency(new string[] { "Tilesets", "Valid", "PrimaryTilesets", "SecondaryTilesets", "FilteredTilesets" })]
        public ObservableCollectionEx<DisplayTuple<string, TilesetModel>> Tilesets
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

        public BlockEditorModel(Dictionary<string, LazyReference<Tileset>> tilesets) : base(tilesets)
        {
            _tilesets = new ObservableCollectionEx<DisplayTuple<string, TilesetModel>>(tilesets.Select(p => new DisplayTuple<string, TilesetModel>(p.Key, new TilesetModel(p.Value, this))));
            foreach(DisplayTuple<string, TilesetModel> element in _tilesets)
                element.Value.SelectedPalette = element.Value.Palettes.First();
            PropertyDependencyHandler.Handle(this);
        }

#if DEBUG
        public BlockEditorModel() : this((MockData.MockRomData()).Tilesets)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }
#endif

        #endregion

        #region Events

        private void Tilesets_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName )
            {
                case "Value.Secondary":
                    RaisePropertyChanged("PrimaryTilesets");
                    RaisePropertyChanged("SecondaryTilesets");
                    RaisePropertyChanged("FilteredTilesets");
                    foreach (DisplayTuple<string, TilesetModel> entry in _tilesets)
                    {
                        entry.Value.RaisePropertyChanged("AdditionalDesignerTileset");
                        entry.Value.RaisePropertyChanged("ValidBlocks");
                    }
                    break;
                case "Value.Valid":
                    RaisePropertyChanged("Valid");
                    break;
            }
        }

        private void Tilesets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("Tilesets");
            RaisePropertyChanged("PrimaryTilesets");
            RaisePropertyChanged("SecondaryTilesets");
            RaisePropertyChanged("FilteredTilesets");
            foreach (DisplayTuple<string, TilesetModel> entry in _tilesets)
            {
                entry.Value.RaisePropertyChanged("AdditionalDesignerTileset");
                entry.Value.RaisePropertyChanged("ValidBlocks");
            }
            RaisePropertyChanged("Valid");
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
