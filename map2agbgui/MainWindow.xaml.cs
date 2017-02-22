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
using map2agbgui.Models;

namespace map2agbgui
{
 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AboutMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.MainViewModel;
            //For Debugging mock
            App.MainViewModel.PopulateDesignerData();
            App.MainViewModel.Status = "Data loaded while runtime";
        }

        private void AddMapButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BlockeditorButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MapTreeViewElement_DoubleClick(object sender, RoutedEventArgs e)
        {
            NumericDisplayTuple<IMapModel> selected = (NumericDisplayTuple<IMapModel>)MapTreeView.SelectedItem;
            if (selected.Value.Mode == MapEntryType.Map)
            {
                MessageBox.Show(((MapModel)selected.Value).Name.ToString());
            }
        }

    }
}
