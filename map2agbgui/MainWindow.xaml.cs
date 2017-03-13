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
using map2agblib.Data;
using System.IO;
using System.Windows.Interop;
using map2agblib.Map;
using map2agbgui.Models;
using map2agbgui.Models.Main.Maps;

using Win32Forms = System.Windows.Forms;

namespace map2agbgui
{

    public partial class MainWindow : Window
    {

        #region Private variables

        private string lastSaveLocation = null;
        private Win32Forms.FolderBrowserDialog projectBrowseDialog;
        private Win32Forms.NativeWindow win32Window;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            RomData romData = MockData.MockRomData();
            App.MainViewModel = new MainModel(romData);
#else
            RomData romData = new RomData();
            App.MainViewModel = new MainModel(romData);
#endif
            DataContext = App.MainViewModel;
            projectBrowseDialog = new Win32Forms.FolderBrowserDialog();
            projectBrowseDialog.RootFolder = Environment.SpecialFolder.Desktop;
            projectBrowseDialog.ShowNewFolderButton = true;
        }

        #endregion

        #region Eventhandler

        #region Eventhandler Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            win32Window = new Win32Forms.NativeWindow();
            win32Window.AssignHandle((new WindowInteropHelper(this)).Handle);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = !SaveProjectAs(lastSaveLocation);
                return;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                App.MainViewModel.Dispose();
            }
        }

        #endregion

        #region Eventhandler Mainmenu

        private void HelpCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadProject();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveProjectAs(lastSaveLocation);
        }

        private void SaveAsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SaveProjectAs(null);
        }

        #endregion

        #region Eventhandler Secondary menubar

        private void AddBankButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.Banks.Add(new DisplayTuple<int, IBankModel>(App.MainViewModel.Banks.Count, new BankModel(new List<LazyReference<MapHeader>>(), App.MainViewModel)));
        }

        private void NamespaceeditorButton_Click(object sender, RoutedEventArgs e)
        {
            NSEditorWindow nSEditorWindow = new NSEditorWindow(App.MainViewModel.NSEditorViewModel);
            nSEditorWindow.Owner = this;
            nSEditorWindow.ShowDialog();
        }

        private void BlockeditorButton_Click(object sender, RoutedEventArgs e)
        {
            BlockEditorWindow blockEditorWindow = new BlockEditorWindow(App.MainViewModel.BlockEditorViewModel);
            blockEditorWindow.Owner = this;
            blockEditorWindow.ShowDialog();
        }

        #endregion

        #region Eventhandler Treeview contextmenu

        #region Maps

        private void MoveMapUpContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IMapModel> selected = (DisplayTuple<int, IMapModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            BankModel currentBank = selected.Value.Bank;
            int index = currentBank.Maps.IndexOf(selected);
            if (index > 0)
            {
                DisplayTuple<int, IMapModel> target = selected.Value.Bank.Maps[index - 1];
                target.Index += 1;
                selected.Index -= 1;
                currentBank.Maps[index - 1] = selected.Value.Bank.Maps[index];
                currentBank.Maps[index] = target;
            }
        }

        private void MoveMapDownContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IMapModel> selected = (DisplayTuple<int, IMapModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            BankModel currentBank = selected.Value.Bank;
            int index = currentBank.Maps.IndexOf(selected);
            if(index < selected.Value.Bank.Maps.Count - 1)
            {
                DisplayTuple<int, IMapModel> target = selected.Value.Bank.Maps[index + 1];
                target.Index -= 1;
                selected.Index += 1;
                currentBank.Maps[index + 1] = selected.Value.Bank.Maps[index];
                currentBank.Maps[index] = target;
            }
        }

        private void ReplaceMapContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IMapModel> selected = (DisplayTuple<int, IMapModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBoxResult.Cancel;
            result = MessageBox.Show("Replace this map with a new one?", "Replace map", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if(result == MessageBoxResult.OK) selected.Value = new MapHeaderModel(new LazyReference<MapHeader>(new MapHeader()), selected.Value.Bank, App.MainViewModel);
        }

        private void RemoveMapContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IMapModel> selected = (DisplayTuple<int, IMapModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBoxResult.Cancel;
            if (selected.Value.EntryMode == MapEntryType.Map) result = MessageBox.Show
                     ("You are about to delete a map. Do you want to replace it with a placeholder?", "Delete map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            else result = MessageBox.Show
                    ("You are about to delete a map placeholder. Update indices?", "Delete map placeholder", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.Cancel)
            {
                if (result == MessageBoxResult.Yes) selected.Value = new NullpointerMapModel(selected.Value.Bank);
                else if (result == MessageBoxResult.No || result == MessageBoxResult.OK)
                {
                    var bank = selected.Value.Bank;
                    bank.Maps.Remove(selected);
                    for (int i = 0; i < bank.Maps.Count; i++) bank.Maps[i].Index = i;
                }
            }
        }

        #endregion

        #region Banks

        private void MoveBankUpContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IBankModel> selected = (DisplayTuple<int,IBankModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            int index = App.MainViewModel.Banks.IndexOf(selected);
            if (index > 0)
            {
                DisplayTuple<int, IBankModel> target = App.MainViewModel.Banks[index - 1];
                target.Index += 1;
                selected.Index -= 1;
                App.MainViewModel.Banks[index - 1] = App.MainViewModel.Banks[index];
                App.MainViewModel.Banks[index] = target;
            }
        }

        private void MoveBankDownContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IBankModel> selected = (DisplayTuple<int, IBankModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            int index = App.MainViewModel.Banks.IndexOf(selected);
            if (index < App.MainViewModel.Banks.Count - 1)
            {
                DisplayTuple<int, IBankModel> target = App.MainViewModel.Banks[index + 1];
                target.Index -= 1;
                selected.Index += 1;
                App.MainViewModel.Banks[index + 1] = App.MainViewModel.Banks[index];
                App.MainViewModel.Banks[index] = target;
            }
        }

        private void AddMapContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IBankModel> selected = (DisplayTuple<int, IBankModel>)MapTreeView.SelectedItem;
            if (selected == null || selected.Value.EntryMode == BankEntryType.Nullpointer) return;
            ((BankModel)selected.Value).Maps.Add(new DisplayTuple<int, IMapModel>(((BankModel)selected.Value).Maps.Count, new MapHeaderModel(new LazyReference<MapHeader>(new MapHeader()), ((BankModel)selected.Value), App.MainViewModel)));
        }

        private void RemoveBankContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IBankModel> selected = (DisplayTuple<int, IBankModel>)MapTreeView.SelectedItem;
            if (selected == null) return;
            MessageBoxResult result = MessageBoxResult.Cancel;
            if (selected.Value.EntryMode == BankEntryType.Bank) result = MessageBox.Show
                     ("You are about to delete a bank. Do you want to replace it with a placeholder?", "Delete bank", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            else result = MessageBox.Show
                    ("You are about to delete a bank placeholder. Update indices?", "Delete bank placeholder", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.Cancel)
            {
                if (result == MessageBoxResult.Yes) selected.Value = new NullpointerBankModel();
                else if (result == MessageBoxResult.No || result == MessageBoxResult.OK)
                {
                    App.MainViewModel.Banks.Remove(selected);
                    for (int i = 0; i < App.MainViewModel.Banks.Count; i++) App.MainViewModel.Banks[i].Index = i;
                }
            }
        }

        private void ReplaceEmptyMapContextEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayTuple<int, IBankModel> selected = (DisplayTuple<int, IBankModel>)MapTreeView.SelectedItem;
            if (selected == null || selected.Value.EntryMode == BankEntryType.Bank) return;
            MessageBoxResult result = MessageBoxResult.Cancel;
            result = MessageBox.Show("Replace this bank placeholder with a real one?", "Replace map", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK) selected.Value = new BankModel(new List<LazyReference<MapHeader>>(), App.MainViewModel);
        }

        #endregion

        #endregion

        #endregion

        #region TreeView extensions

        private void MapTreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem)) source = VisualTreeHelper.GetParent(source);
            return source as TreeViewItem;
        }

        #endregion

        #region Methods

        private bool LoadProject()
        {
            projectBrowseDialog.SelectedPath = "";
            projectBrowseDialog.Description = "Open project from directory";
            Win32Forms.DialogResult result = projectBrowseDialog.ShowDialog(win32Window);
            string dirPath = projectBrowseDialog.SelectedPath;
            if (dirPath == "" || result == Win32Forms.DialogResult.Abort || result == Win32Forms.DialogResult.Cancel) return false;
            if (!Directory.Exists(dirPath)) return false;
            RomData romData = null;
            try
            {
                romData = RomData.ImportFromDirectory(dirPath);
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error opening project", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            App.MainViewModel = new MainModel(romData);
            DataContext = App.MainViewModel;
            Title = "map2agb - " + dirPath;
            lastSaveLocation = dirPath;
            return true;
        }

        private bool SaveProjectAs(string saveName)
        {
            if(!App.MainViewModel.Valid)
            {
                MessageBoxResult result = MessageBox.Show("The project has some errors. Save anyway?", "Errors in project", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (result == MessageBoxResult.Cancel) return false;
            }
            if(saveName == null)
            {
                projectBrowseDialog.SelectedPath = "";
                projectBrowseDialog.Description = "Save project to directory";
                Win32Forms.DialogResult result = projectBrowseDialog.ShowDialog(win32Window);
                saveName = projectBrowseDialog.SelectedPath;
                if (saveName == "" || result == Win32Forms.DialogResult.Abort || result == Win32Forms.DialogResult.Cancel) return false;
            }
            RomData romData = App.MainViewModel.ToRomData();
            try
            {
                RomData.ExportToDirectory(romData, saveName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error saving project", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            Title = "map2agb - " + saveName;
            lastSaveLocation = saveName;
            return true;
        }


        #endregion

       
    }
}
