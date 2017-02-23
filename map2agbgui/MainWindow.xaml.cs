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
using System.Windows.Navigation;
using System.Windows.Shapes;
using map2agbgui.Models.Main;
using map2agbgui.Dialogs;
using map2agblib.Data;

namespace map2agbgui
{
 
    public partial class MainWindow : Window
    {

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            RomData romData = MockData.MockRomData(); //For debugging
            App.MainViewModel = new MainModel(romData);
            DataContext = App.MainViewModel;
            App.MainViewModel.Status = "Data loaded while runtime";
        }

        #endregion

        #region Eventhandler Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Eventhandler Mainmenu

        private void AboutMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Eventhandler Secondary menubar

        private void AddMapButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveMapButton_Click(object sender, RoutedEventArgs e)
        {
            NumericDisplayTuple<IMapModel> selected = (NumericDisplayTuple<IMapModel>)MapTreeView.SelectedItem;
            DeleteMapDialog deleteMapDialog = new DeleteMapDialog(selected.Value.EntryMode == MapEntryType.Map);
            deleteMapDialog.Owner = this;
            bool result = (bool)deleteMapDialog.ShowDialog();
            if(result)
            {
                switch (deleteMapDialog.DeleteChoiceResult)
                {
                    case DeleteMapDialog.DeleteMapChoice.ReplaceNullpointer:
                        selected.Value = new NullpointerMapModel(selected.Value.Bank);
                        break;

                    case DeleteMapDialog.DeleteMapChoice.UpdateIndices:
                        var bank = selected.Value.Bank;
                        bank.Maps.Remove(selected);
                        for (int i = 0; i < bank.Maps.Count; i++) bank.Maps[i].Index = i;
                        App.MainViewModel.RaisePropertyChanged("Banks");
                        break;
                }
            }
        }

        private void NamespaceeditorButton_Click(object sender, RoutedEventArgs e)
        {
            NSEditorWindow nSEditorWindow = new NSEditorWindow(App.MainViewModel.NSEditorViewModel);
            nSEditorWindow.Owner = this;
            nSEditorWindow.ShowDialog();
        }

        private void BlockeditorButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

    }
}
