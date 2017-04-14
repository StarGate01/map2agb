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
using map2agbgui.Models.Dialogs;

namespace map2agbgui
{

    public partial class ImportDialogWindow : Window
    {

        public ImportDialogModel DataModel;

        private System.Windows.Forms.OpenFileDialog loadROMDialog;

        #region Constructor

        public ImportDialogWindow(ImportDialogModel model)
        {
            InitializeComponent();
            DataModel = model;
            DataContext = DataModel;
            loadROMDialog = new System.Windows.Forms.OpenFileDialog();
            loadROMDialog.CheckFileExists = true;
            loadROMDialog.DefaultExt = "gba";
            loadROMDialog.Filter = "GBA ROMs|*.gba";
            loadROMDialog.Multiselect = false;
            loadROMDialog.ShowHelp = false;
            loadROMDialog.Title = "Import from ROM";
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

        #region Button Eventhandler

        private void SelectROMButton_Click(object sender, RoutedEventArgs e)
        {
            loadROMDialog.FileName = "";
            System.Windows.Forms.DialogResult result = loadROMDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Abort || result == System.Windows.Forms.DialogResult.Cancel) return;
            DataModel.ROMPath = loadROMDialog.FileName;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        #endregion

    }

}
