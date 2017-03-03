using map2agblib.Tilesets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using map2agblib.Data;
using System.Collections.ObjectModel;
using map2agbgui.Models.Main;
using map2agblib.Imaging;
using System.Diagnostics;
using map2agbgui.Extensions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetModel : IRomSerializable<TilesetModel, LazyReference<Tileset>>, ITupleFormattable, INotifyPropertyChanged
    {

        #region Native

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        #endregion

        #region Properties

        #region Meta properties

        private BlockEditorModel _blockEditorViewModel;
        public BlockEditorModel BlockEditorViewModel
        {
            get
            {
                return _blockEditorViewModel;
            }
            set
            {
                _blockEditorViewModel = value;
                RaisePropertyChanged("BlockEditorViewModel");
            }
        }

        public string FormatString
        {
            get
            {
                return "{0}";
            }
        }

        public bool Valid
        {
            get
            {
                return ValidSettings && ValidBlocks;
            }
        }

        public bool ValidSettings
        {
            get
            {
                return ValidImage;
            }
        }

        public bool ValidAdditionalDesignerTileset
        {
            get
            {
                return _blockEditorViewModel.Tilesets.Any(p => p.Index == _additionalDesignerTileset && p.Value.Secondary != _secondary);
            }
        }

        public bool ValidBlocks
        {
            get
            {
                return ValidAdditionalDesignerTileset;
            }
        }

        private string _additionalDesignerTileset;
        public string AdditionalDesignerTileset
        {
            get
            {
                return _additionalDesignerTileset;
            }
            set
            {
                _additionalDesignerTileset = value;
                RaisePropertyChanged("AdditionalDesignerTileset");
                RaisePropertyChanged("ValidAdditionalDesignerTileset");
                RaisePropertyChanged("ValidBlocks");
                RaisePropertyChanged("Valid");
            }
        }

        #endregion

        #region Data properties

        private string _graphicPath;
        public string GraphicPath
        {
            get
            {
                return _graphicPath;
            }
            set
            {
                _graphicPath = value;
                _graphicBuffer = null;
                _graphicBaseBuffer = null;
                RaisePropertyChanged("GraphicPath");
                RaisePropertyChanged("BaseGraphic");
                RaisePropertyChanged("Graphic");
                RaisePropertyChanged("ValidImage");
                RaisePropertyChanged("ValidSettings");
                RaisePropertyChanged("Valid");
            }
        }
        public DisplayTuple<int, PaletteModel> _selectedPalette;
        public DisplayTuple<int, PaletteModel> SelectedPalette
        {
            get
            {
                return _selectedPalette;
            }
            set
            {
                _selectedPalette = value;
                _graphicBuffer = null;
                RaisePropertyChanged("SelectedPalette");
                RaisePropertyChanged("Graphic");
            }
        }

        private WriteableBitmap _graphicBaseBuffer, _graphicBuffer;
        public WriteableBitmap Graphic
        {
            get
            {
                if (ValidImage)
                {
                    if(_graphicBaseBuffer == null) _graphicBaseBuffer = new WriteableBitmap(new BitmapImage(new Uri(_graphicPath, UriKind.Absolute)));
                    if (_graphicBuffer == null) _graphicBuffer = PaletteShader(_graphicBaseBuffer, _selectedPalette?.Value);
                    return _graphicBuffer;
                }
                else return null;
            }
        }
        public bool ValidImage
        {
            get
            {
                return _graphicPath != null & File.Exists(_graphicPath);
            }
        }

        private bool _compressed, _secondary;
        public bool Compressed
        {
            get
            {
                return _compressed;
            }
            set
            {
                _compressed = value;
                RaisePropertyChanged("Compressed");
            }
        }
        public bool Secondary
        {
            get
            {
                return _secondary;
            }
            set
            {
                _secondary = value;
                RaisePropertyChanged("Secondary");
            }
        }

        private byte _field2, _field3;
        public byte Field2
        {
            get
            {
                return _field2;
            }
            set
            {
                _field2 = value;
                RaisePropertyChanged("Field2");
            }
        }
        public byte Field3
        {
            get
            {
                return _field3;
            }
            set
            {
                _field3 = value;
                RaisePropertyChanged("Field3");
            }
        }

        private ObservableCollection<DisplayTuple<int, PaletteModel>> _palettes;
        public ObservableCollection<DisplayTuple<int, PaletteModel>> Palettes
        {
            get
            {
                return _palettes;
            }
            set
            {
                _palettes = value;
                RaisePropertyChanged("Palettes");
            }
        }

        private ObservableCollection<TilesetEntryModel> _blocks;
        public ObservableCollection<TilesetEntryModel> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                RaisePropertyChanged("Blocks");
            }
        }

        #endregion

        #endregion

        #region Constructor

        public TilesetModel(LazyReference<Tileset> tileset, BlockEditorModel parentEditorModel) : base(tileset)
        {
            _graphicPath = tileset.Data.Graphic;
            _compressed = tileset.Data.Compressed;
            _secondary = tileset.Data.Secondary;
            _field2 = tileset.Data.Field2;
            _field3 = tileset.Data.Field3;
            _palettes = new ObservableCollectionEx<DisplayTuple<int, PaletteModel>>(tileset.Data.Palettes.Select((p, pi) =>
                new DisplayTuple<int, PaletteModel>(pi, new PaletteModel(p))));
            _blocks = new ObservableCollection<TilesetEntryModel>(tileset.Data.Blocks.Select(p => new TilesetEntryModel(p)));
            _selectedPalette = _palettes[0];
            _blockEditorViewModel = parentEditorModel;
        }

        #endregion

        #region Methods

        public override LazyReference<Tileset> ToRomData()
        {
            Tileset tileset = new Tileset();
            tileset.Graphic = _graphicPath;
            tileset.Compressed = _compressed;
            tileset.Secondary = false;
            tileset.Field2 = _field2;
            tileset.Field3 = _field3;
            tileset.Palettes = _palettes.Select(p => p.Value.ToRomData()).ToArray();
            tileset.Blocks = _blocks.Select(p => p.ToRomData()).ToArray();
            return new LazyReference<Tileset>(tileset);
        }

        private WriteableBitmap PaletteShader(WriteableBitmap bitmap, PaletteModel palette)
        {
            if (palette == null) return bitmap;
            WriteableBitmap newbitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, palette.ToBitmapPalette());
            uint bufferLen = (uint)(bitmap.PixelHeight * bitmap.PixelWidth * bitmap.Format.BitsPerPixel) / 8;
            newbitmap.Lock();
            bitmap.Lock();
            memcpy(newbitmap.BackBuffer, bitmap.BackBuffer, (UIntPtr)bufferLen);
            bitmap.Unlock();
            newbitmap.AddDirtyRect(new Int32Rect(0, 0, newbitmap.PixelWidth, newbitmap.PixelHeight));
            newbitmap.Unlock();
            return newbitmap;
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
