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
using map2agbgui.Models.NSEditor;
using map2agbgui.Native;

namespace map2agbgui
{

    public partial class NSEditorWindow : Window
    {

        NSEditorModel dataModel;

        public NSEditorWindow(NSEditorModel model)
        {
            InitializeComponent();
            dataModel = model;
            DataContext = dataModel;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowMods.RemoveMaximizeBox(this);
            WindowMods.RemoveIcon(this);
        }

    }

}
