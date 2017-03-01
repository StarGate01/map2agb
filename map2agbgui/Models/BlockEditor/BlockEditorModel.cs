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

namespace map2agbgui.Models.BlockEditor
{

    public class BlockEditorModel : INotifyPropertyChanged
    {

        #region Properties

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

        #endregion

        #region Constructor

        public BlockEditorModel(Dictionary<string, LazyReference<Tileset>> tilesets)
        {
            _tilesets = new ObservableCollection<DisplayTuple<string, TilesetModel>>(tilesets.Select(p => new DisplayTuple<string, TilesetModel>(p.Key, new TilesetModel(p.Value))));
        }

        public BlockEditorModel() : this((MockData.MockRomData()).Tilesets)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public Dictionary<string, LazyReference<Tileset>> ToRomData()
        {
           return _tilesets.ToDictionary(k => k.Index, p => new LazyReference<Tileset>(p.Value.ToRomData()));
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public BlockEditorModel GetCopy()
        {
            BlockEditorModel copy = (BlockEditorModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}
