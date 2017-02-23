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

namespace map2agbgui.Dialogs
{

    public partial class DeleteMapDialog : Window
    {

        #region Enums

        public enum DeleteMapChoice { Cancel, ReplaceNullpointer, UpdateIndices }

        #endregion

        #region Properties

        public DeleteMapChoice DeleteChoiceResult { get; private set; } = DeleteMapChoice.Cancel;

        #endregion

        #region Constructors

        public DeleteMapDialog(bool showReplaceButton)
        {
            InitializeComponent();
            if (!showReplaceButton) ButtonReplace.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Eventhandler Window

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowMods.RemoveIcon(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonCancel.Focus();
        }

        #endregion

        #region Eventhandler Buttons

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            DeleteChoiceResult = DeleteMapChoice.UpdateIndices;
            Close();
        }

        private void ButtonReplace_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            DeleteChoiceResult = DeleteMapChoice.ReplaceNullpointer;
            Close();
        }

        #endregion

    }

}
