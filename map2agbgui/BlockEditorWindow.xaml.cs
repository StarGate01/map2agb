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
using map2agblib.Imaging.JASCPAL;

namespace map2agbgui
{

    public partial class BlockEditorWindow : Window
    {

        BlockEditorModel dataModel;
        System.Windows.Forms.OpenFileDialog loadPalDialog;

        #region Constructor

        public BlockEditorWindow(BlockEditorModel model)
        {
            InitializeComponent();
            dataModel = model;
            DataContext = dataModel;
            loadPalDialog = new System.Windows.Forms.OpenFileDialog();
            loadPalDialog.CheckFileExists = true;
            loadPalDialog.DefaultExt = "pal";
            loadPalDialog.Filter = "Palette files|*.pal";
            loadPalDialog.Multiselect = false;
            loadPalDialog.ShowHelp = false;
            loadPalDialog.Title = "Import palette";
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

        private void EditPaletteContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, PaletteModel> selected = (DisplayTuple<int, PaletteModel>)PaletteListBox.SelectedItem;
            if (selected == null) return;
            PaletteEditorWindow paletteEditorWindow = new PaletteEditorWindow(selected.Value);
            paletteEditorWindow.Owner = this;
            paletteEditorWindow.ShowDialog();
        }

        private void RemovePaletteContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, PaletteModel> selected = (DisplayTuple<int, PaletteModel>)PaletteListBox.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBox.Show("Delete this palette?", "Delete palette", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Cancel) return;
            DisplayTuple<string, TilesetModel> currentTileset = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            currentTileset.Value.Palettes.Remove(selected);
            for (int i = 0; i < currentTileset.Value.Palettes.Count; i++) currentTileset.Value.Palettes[i].Index = i;
        }

        private void AddPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<string, TilesetModel> currentTileset = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (currentTileset == null) return;
            currentTileset.Value.Palettes.Add(new DisplayTuple<int, PaletteModel>(currentTileset.Value.Palettes.Count, new PaletteModel(new Palette())));
        }

        private void ImportPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<string, TilesetModel> currentTileset = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (currentTileset == null) return;
            loadPalDialog.FileName = "";
            System.Windows.Forms.DialogResult result = loadPalDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                Palette pal = JASCPALImport.Import(loadPalDialog.FileName);
                currentTileset.Value.Palettes.Add(new DisplayTuple<int, PaletteModel>(currentTileset.Value.Palettes.Count, new PaletteModel(pal)));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
