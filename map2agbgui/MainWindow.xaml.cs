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
            RomData romData = MockData.MockRomData(); //For debugging
            App.MainViewModel = new MainModel(romData);
            DataContext = App.MainViewModel;
            App.MainViewModel.Status = "Data loaded while runtime";
            projectBrowseDialog = new Win32Forms.FolderBrowserDialog();
            projectBrowseDialog.RootFolder = Environment.SpecialFolder.Desktop;
            projectBrowseDialog.ShowNewFolderButton = true;
        }

        #endregion

        #region Eventhandler Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            win32Window = new Win32Forms.NativeWindow();
            win32Window.AssignHandle((new WindowInteropHelper(this)).Handle);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Yes) e.Cancel = !SaveProjectAs(lastSaveLocation);
            else if (result == MessageBoxResult.Cancel) e.Cancel = true;
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

        private void AddMapButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveMapButton_Click(object sender, RoutedEventArgs e)
        {
            NumericDisplayTuple<IMapModel> selected = (NumericDisplayTuple<IMapModel>)MapTreeView.SelectedItem;
            MessageBoxResult result = MessageBoxResult.Cancel;
            if (selected.Value.EntryMode == MapEntryType.Map) result = MessageBox.Show
                     ("You are about to delete a map. Do you want to replace it with a placeholder?", "Delete map", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            else result = MessageBox.Show
                    ("You are about to delete a map placeholder. Update indices?", "Delete map placeholder", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if(result != MessageBoxResult.Cancel)
            {
                if (result == MessageBoxResult.Yes)
                {
                    selected.Value = new NullpointerMapModel(selected.Value.Bank);
                }
                else if (result == MessageBoxResult.No || result == MessageBoxResult.OK)
                { 
                    var bank = selected.Value.Bank;
                    bank.Maps.Remove(selected);
                    for (int i = 0; i < bank.Maps.Count; i++) bank.Maps[i].Index = i;
                    App.MainViewModel.RaisePropertyChanged("Banks");
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
            App.MainViewModel.Status = "Project loaded";
            Title = "map2agb - " + dirPath;
            lastSaveLocation = dirPath;
            return true;
        }

        private bool SaveProjectAs(string saveName)
        {
            if(saveName == null)
            {
                projectBrowseDialog.SelectedPath = "";
                projectBrowseDialog.Description = "Save project to directory";
                Win32Forms.DialogResult result = projectBrowseDialog.ShowDialog(win32Window);
                saveName = projectBrowseDialog.SelectedPath;
                if (saveName == "" || result == Win32Forms.DialogResult.Abort || result == Win32Forms.DialogResult.Cancel) return false;
            }
            RomData romData = App.MainViewModel.SaveToRomData();
            try
            {
                RomData.ExportToDirectory(romData, saveName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error saving project", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            App.MainViewModel.Status = "Project saved";
            Title = "map2agb - " + saveName;
            lastSaveLocation = saveName;
            return true;
        }

        #endregion


    }
}
