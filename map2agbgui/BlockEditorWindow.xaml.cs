using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using map2agbgui.Native;
using map2agbgui.Models.BlockEditor;
using map2agbgui.Models.Main;
using map2agblib.Imaging;
using map2agblib.Data;
using map2agblib.Tilesets;

namespace map2agbgui
{

    public partial class BlockEditorWindow : Window
    {

        BlockEditorModel dataModel;

        #region Constructor

        public BlockEditorWindow(BlockEditorModel model)
        {
            InitializeComponent();
            dataModel = model;
            DataContext = dataModel;
        }

        #endregion

        #region Window Eventhandler

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowMods.RemoveMaximizeBox(this);
            WindowMods.RemoveMinimizeBox(this);
            WindowMods.RemoveIcon(this);
        }

        #endregion

        #region Palettes Eventhandler

        private void PaletteListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DisplayTuple<int, PaletteModel> selected = (DisplayTuple<int, PaletteModel>)PaletteListBox.SelectedItem;
            if (selected == null) return;
            PaletteEditorWindow paletteEditorWindow = new PaletteEditorWindow(selected.Value, selected.Index);
            paletteEditorWindow.Owner = this;
            paletteEditorWindow.ShowDialog();
        }

        #endregion

        #region Tilests Eventhandler

        private void AddTilesetButton_Click(object sender, RoutedEventArgs e)
        {
            string namePrefix = "NONAME";
            int nameCounter = 0;
            while(true)
            {
                if (!dataModel.Tilesets.ToList().Exists(p => p.Index == namePrefix + nameCounter)) break;
                nameCounter++;
            }
            dataModel.Tilesets.Add(new DisplayTuple<string, TilesetModel>(namePrefix + nameCounter, new TilesetModel(new LazyReference<Tileset>(new Tileset()))));
        }

        private void RemoveTilesetContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<string, TilesetModel> selected = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBox.Show("Delete this tileset?", "Delete tileset", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Cancel) return;
            dataModel.Tilesets.Remove(selected);
        }

        #endregion

    }

}
