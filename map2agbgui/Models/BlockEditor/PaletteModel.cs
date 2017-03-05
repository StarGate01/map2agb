using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Imaging;
using System.Collections.ObjectModel;
using map2agbgui.Models.Main;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace map2agbgui.Models.BlockEditor
{

    public class PaletteModel : IRomSerializable<PaletteModel, Palette>, INotifyPropertyChanged, ITupleFormattable
    {

        #region Properties
        public string FormatString
        {
            get
            {
                return "{1} {0}";
            }
        }

        private ObservableCollection<ShortColorModel> _colors;
        public ObservableCollection<ShortColorModel> Colors
        {
            get
            {
                return _colors;
            }
            set
            {
                _colors = value;
                RefreshTexture();
                RaisePropertyChanged("Colors");
            }
        }

        private WriteableBitmap _texture = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/map2agbgui;component/Assets/TestPalette_16x1.bmp")));
        private ImageBrush _textureBrush;
        public ImageBrush Texture
        {
            get
            {
                if(_textureBrush == null) _textureBrush = new ImageBrush(_texture);
                return _textureBrush;
            }
        }
        public WriteableBitmap TextureSource
        {
            get
            {
                return _texture;
            }
        }

        public unsafe void RefreshTexture()
        {
            _texture.Lock();
            byte* data = (byte*)_texture.BackBuffer;
            for (int i=0; i<16; i++)
            {
                data[i * 4] = (byte)(_colors[i].Blue << 3);
                data[(i * 4) + 1] = (byte)(_colors[i].Green << 3);
                data[(i * 4) + 2] = (byte)(_colors[i].Red << 3);
            }
            _texture.AddDirtyRect(new Int32Rect(0, 0, 16, 1));
            _texture.Unlock();
        }

        #endregion

        #region Constructor

        public PaletteModel(Palette palette) : base(palette)
        {
            _colors = new ObservableCollection<ShortColorModel>(palette.Colors.Select(p => new ShortColorModel(p)));
            RefreshTexture();
        }

#if DEBUG
        public PaletteModel() : base(null)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                _colors = new ObservableCollection<ShortColorModel>((MockData.MockRomData()).Tilesets.First().Value.Data.Palettes.First().Colors.Select(p => new ShortColorModel(p)));
                RefreshTexture();
            }
        }
#endif

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Palette";
        }

        public override Palette ToRomData()
        {
            return new Palette(_colors.Select(p => p.ToRomData()).ToArray());
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
