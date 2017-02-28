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
using map2agblib.IO;
using System.IO;

namespace map2agbgui
{

    public partial class MainWindow : Window
    {

        #region Private variables

        private string lastSaveLocation = null;
        private Microsoft.Win32.OpenFileDialog openProjectDlg;
        private Microsoft.Win32.SaveFileDialog saveProjectDlg;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            RomData romData = MockData.MockRomData(); //For debugging
            App.MainViewModel = new MainModel(romData);
            DataContext = App.MainViewModel;
            App.MainViewModel.Status = "Data loaded while runtime";
            openProjectDlg = new Microsoft.Win32.OpenFileDialog();
            openProjectDlg.CheckFileExists = true;
            openProjectDlg.DefaultExt = ImportExport.FILE_EXT;
            openProjectDlg.Filter = "Project files|*." + ImportExport.FILE_EXT;
            openProjectDlg.Title = "Open project file";
            saveProjectDlg = new Microsoft.Win32.SaveFileDialog();
            saveProjectDlg.OverwritePrompt = true;
            saveProjectDlg.DefaultExt = ImportExport.FILE_EXT;
            saveProjectDlg.Filter = "Project files|*." + ImportExport.FILE_EXT;
            saveProjectDlg.Title = "Save project file";
        }

        #endregion

        #region Eventhandler Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
            openProjectDlg.FileName = "";
            bool result = (bool)openProjectDlg.ShowDialog(this);
            string fileName = openProjectDlg.FileName;
            if (fileName == "" || !result) return false;
            if (!File.Exists(fileName)) return false;
            RomData romData = null;
            try
            {
                romData = ImportExport.ImportFromFile(fileName);
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error opening project", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            App.MainViewModel = new MainModel(romData);
            DataContext = App.MainViewModel;
            App.MainViewModel.Status = "Project loaded";
            Title = "map2agb - " + System.IO.Path.GetFileName(fileName);
            lastSaveLocation = fileName;
            return true;
        }

        private bool SaveProjectAs(string saveName)
        {
            if(saveName == null)
            {
                saveProjectDlg.FileName = "";
                bool result = (bool)saveProjectDlg.ShowDialog(this);
                saveName = saveProjectDlg.FileName;
                if (saveName == "" || !result) return false;
            }
            RomData romData = App.MainViewModel.SaveToRomData();
            try
            {
                ImportExport.ExportToFile(romData, saveName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error opening project", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            App.MainViewModel.Status = "Project saved";
            Title = "map2agb - " + System.IO.Path.GetFileName(saveName);
            lastSaveLocation = saveName;
            return true;
        }

        #endregion


    }
}
