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

        PaletteModel dataModel;
        PaletteModel backup;
        System.Windows.Forms.OpenFileDialog loadPalDialog;
        System.Windows.Forms.SaveFileDialog savePalDialog;

        #region Constructor

        public PaletteEditorWindow(PaletteModel model, int displayIndex)
        {
            InitializeComponent();
            dataModel = model;
            DataContext = dataModel;
            backup = (PaletteModel)dataModel.Clone();
            Title += " - Palette " + displayIndex;
            loadPalDialog = new System.Windows.Forms.OpenFileDialog();
            loadPalDialog.CheckFileExists = true;
            loadPalDialog.DefaultExt = "pal";
            loadPalDialog.Filter = "Palette files|*.pal";
            loadPalDialog.Multiselect = false;
            loadPalDialog.ShowHelp = false;
            loadPalDialog.Title = "Import palette";
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
                dataModel.Colors = pal.Colors;
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
            dataModel.Colors = backup.Colors;
            ColorListBox.SelectedIndex = 0;
        }

        #endregion

    }

}
