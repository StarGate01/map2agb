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
using System.ComponentModel;

namespace map2agbgui
{

    public partial class BlockEditorWindow : Window
    {

        BlockEditorModel dataModel;
        System.Windows.Forms.OpenFileDialog loadGraphicDialog;

        #region Constructor

        public BlockEditorWindow(BlockEditorModel model)
        {
            InitializeComponent();
            dataModel = model;
            DataContext = dataModel;
            loadGraphicDialog = new System.Windows.Forms.OpenFileDialog();
            loadGraphicDialog.CheckFileExists = true;
            loadGraphicDialog.DefaultExt = "png";
            loadGraphicDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
            loadGraphicDialog.Multiselect = false;
            loadGraphicDialog.ShowHelp = false;
            loadGraphicDialog.Title = "Select image";
            TilesetListBox.Items.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));
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
            DisplayTuple<string, TilesetModel> selectedTileset = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (selectedTileset == null) return;
            DisplayTuple<int, PaletteModel> selected = (DisplayTuple<int, PaletteModel>)PaletteListBox.SelectedItem;
            if (selected == null) return;
            PaletteEditorWindow paletteEditorWindow = new PaletteEditorWindow(selected.Value, selected.Index);
            paletteEditorWindow.Owner = this;
            bool result = (bool)paletteEditorWindow.ShowDialog();
            if (result && paletteEditorWindow.DialogDataResult != null)
            {
                int selectedIndex = PaletteListBox.SelectedIndex;
                selected.Value = paletteEditorWindow.DialogDataResult;
                selectedTileset.Value.SelectedPalette = selectedTileset.Value.Palettes[selectedIndex];
            }
        }

        #endregion

        #region Tilesets Eventhandler

        private void AddTilesetButton_Click(object sender, RoutedEventArgs e)
        {
            string namePrefix = "NONAME";
            int nameCounter = 0;
            while(true)
            {
                if (!dataModel.Tilesets.Any(p => p.Index == namePrefix + nameCounter)) break;
                nameCounter++;
            }
            DisplayTuple<string, TilesetModel> newElement = new DisplayTuple<string, TilesetModel>(namePrefix + nameCounter, new TilesetModel(new LazyReference<Tileset>(new Tileset()), dataModel));
            dataModel.Tilesets.Add(newElement);
            TilesetListBox.SelectedItem = newElement;
        }

        private void RemoveTilesetContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<string, TilesetModel> selected = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBox.Show("Delete this tileset?", "Delete tileset", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Cancel) return;
            dataModel.Tilesets.Remove(selected);
        }

        private void ChangeGraphicButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<string,TilesetModel> selected = (DisplayTuple<string, TilesetModel>)TilesetListBox.SelectedItem;
            if (selected == null) return;
            loadGraphicDialog.FileName = "";
            System.Windows.Forms.DialogResult result = loadGraphicDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                BitmapImage img = new BitmapImage(new Uri(loadGraphicDialog.FileName, UriKind.Absolute));
                if (img.Format != PixelFormats.Indexed4) throw new Exception("Image has not 4 indexed bit per pixel");
                selected.Value.GraphicPath = loadGraphicDialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }

}
