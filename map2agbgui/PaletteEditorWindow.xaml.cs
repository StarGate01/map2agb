using map2agbgui.Models.BlockEditor;
using map2agbgui.Native;
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
using map2agblib.Imaging.IO;
using map2agblib.Imaging;
using System.Collections.ObjectModel;

namespace map2agbgui
{

    public partial class PaletteEditorWindow : Window
    {

        private PaletteModel dataModel;
        private PaletteModel backup;
        private System.Windows.Forms.OpenFileDialog loadPalDialog;
        private System.Windows.Forms.OpenFileDialog loadImageDialog;
        private System.Windows.Forms.SaveFileDialog savePalDialog;

        public PaletteModel DialogDataResult = null;

        #region Constructor

        public PaletteEditorWindow(PaletteModel model, int displayIndex)
        {
            InitializeComponent();
            dataModel = model;
            backup = (PaletteModel)dataModel.Clone();
            DataContext = backup;
            Title += " - Palette " + displayIndex;
            loadPalDialog = new System.Windows.Forms.OpenFileDialog();
            loadPalDialog.CheckFileExists = true;
            loadPalDialog.DefaultExt = "pal";
            loadPalDialog.Filter = "JASC-PAL RIFF palette files|*.pal";
            loadPalDialog.Multiselect = false;
            loadPalDialog.ShowHelp = false;
            loadPalDialog.Title = "Import palette from file";
            loadImageDialog = new System.Windows.Forms.OpenFileDialog();
            loadImageDialog.CheckFileExists = true;
            loadImageDialog.DefaultExt = "png";
            loadImageDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
            loadImageDialog.Multiselect = false;
            loadImageDialog.ShowHelp = false;
            loadImageDialog.Title = "Import palette from image";
            savePalDialog = new System.Windows.Forms.SaveFileDialog();
            savePalDialog.OverwritePrompt = true;
            savePalDialog.DefaultExt = "pal";
            savePalDialog.Filter = "Palette files|*.pal";
            savePalDialog.ShowHelp = false;
            savePalDialog.Title = "Export palette";
        }

        #endregion

        #region Eventhandler Window

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowMods.RemoveMaximizeBox(this);
            WindowMods.RemoveMinimizeBox(this);
            WindowMods.RemoveIcon(this);
        }

        #endregion

        #region Eventhandler Menubar

        private void ImportPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            loadPalDialog.FileName = "";
            System.Windows.Forms.DialogResult result = loadPalDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                PaletteModel pal = new PaletteModel(JASCPAL.Import(loadPalDialog.FileName));
                backup.Colors = pal.Colors;
                ColorListBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportImagePaletteButton_Click(object sender, RoutedEventArgs e)
        {
            loadImageDialog.FileName = "";
            System.Windows.Forms.DialogResult result = loadImageDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                BitmapImage image = new BitmapImage(new Uri(loadImageDialog.FileName, UriKind.Absolute));
                if (image.Palette.Colors.Count != 16) throw new Exception("Image has not 16 indexed colors");
                backup.Colors = new ObservableCollection<ShortColorModel>(image.Palette.Colors.Select(p => 
                    new ShortColorModel(new ShortColor((byte)(p.R >> 3), (byte)(p.G >> 3), (byte)(p.B >> 3)))));
                ColorListBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            savePalDialog.FileName = "";
            System.Windows.Forms.DialogResult result = savePalDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            try
            {
                JASCPAL.Export(dataModel.ToRomData(), savePalDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Export error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetPaletteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Discard changes and restore palette?", "Reset palette", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Cancel) return;
            backup.Colors = dataModel.Colors;
            ColorListBox.SelectedIndex = 0;
        }

        #endregion

        #region Eventhandler Buttons

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogDataResult = backup;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

    }

}
