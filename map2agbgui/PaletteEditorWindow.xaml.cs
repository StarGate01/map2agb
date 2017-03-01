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

namespace map2agbgui
{

    public partial class PaletteEditorWindow : Window
    {

        public PaletteEditorWindow(PaletteModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowMods.RemoveMaximizeBox(this);
            WindowMods.RemoveMinimizeBox(this);
            WindowMods.RemoveIcon(this);
        }

    }

}
