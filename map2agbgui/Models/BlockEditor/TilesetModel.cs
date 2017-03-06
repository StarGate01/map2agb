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
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetModel : IRomSerializable<TilesetModel, LazyReference<Tileset>>, ITupleFormattable, IRaisePropertyChanged
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

        [PropertyDependency("Valid")]
        public bool ValidSettings
        {
            get
            {
                return ValidImage;
            }
        }

        [PropertyDependency("Valid")]
        public bool ValidBlocks
        {
            get
            {
                return ValidAdditionalDesignerTileset;
            }
        }

        [PropertyDependency("ValidBlocks")]
        public bool ValidAdditionalDesignerTileset
        {
            get
            {
                return _blockEditorViewModel.Tilesets.Any(p => p.Index == _additionalDesignerTilesetID && p.Value.Secondary != _secondary);
            }
        }

        [PropertyDependency("ValidSettings")]
        public bool ValidImage
        {
            get
            {
                return _graphicPath != null & File.Exists(_graphicPath);
            }
        }

        #endregion

        #region Data properties

        private string _additionalDesignerTilesetID;
        [PropertyDependency(new string[] { "ValidAdditionalDesignerTileset", "AdditionalDesignerTileset", "PalettesTexture" })]
        public string AdditionalDesignerTilesetID
        {
            get
            {
                return _additionalDesignerTilesetID;
            }
            set
            {
                _additionalDesignerTilesetID = value;
                _palettesTextureBrush = null;
                RaisePropertyChanged("AdditionalDesignerTilesetID");
            }
        }
        public DisplayTuple<string, TilesetModel> AdditionalDesignerTileset
        {
            get
            {
                if (!ValidAdditionalDesignerTileset) return null;
                return _blockEditorViewModel.Tilesets.First(p => p.Index == _additionalDesignerTilesetID);
            }
        }

        private string _graphicPath;
        [PropertyDependency(new string[] { "ValidImage", "Graphic", "GraphicTexture" })]
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
                _potGraphicBugger = null;
                RaisePropertyChanged("GraphicPath");
            }
        }

        private WriteableBitmap _graphicBuffer, _potGraphicBugger;
        public BitmapSource Graphic
        {
            get
            {
                if (ValidImage)
                {
                    if (_graphicBuffer == null)
                    {
                        _graphicBuffer = new WriteableBitmap(new BitmapImage(new Uri(_graphicPath, UriKind.Absolute)));
                        PrepareForPaletteShader(ref _graphicBuffer);
                    }
                    return  _graphicBuffer;
                }
                else return null;
            }
        }
        public ImageBrush GraphicTexture
        {
            get
            {
                if (_potGraphicBugger == null)
                {
                    _potGraphicBugger = new WriteableBitmap(new BitmapImage(new Uri(_graphicPath, UriKind.Absolute)));
                    PrepareForPaletteShader(ref _potGraphicBugger);
                    ResizeToPOT(ref _potGraphicBugger);
                }
                return new ImageBrush(_potGraphicBugger);
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
                RaisePropertyChanged("SelectedPalette");
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

        #region Palettes

        private ObservableCollectionEx<DisplayTuple<int, PaletteModel>> _palettes;
        [PropertyDependency("PalettesTexture")]
        public ObservableCollectionEx<DisplayTuple<int, PaletteModel>> Palettes
        {
            get
            {
                return _palettes;
            }
            set
            {
                _palettes = value;
                _palettesTextureBrush = null;
                _palettes.ItemPropertyChanged += Palettes_ItemPropertyChanged;
                RaisePropertyChanged("Palettes");
            }
        }

        private WriteableBitmap _palettesTexture = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/map2agbgui;component/Assets/TestPalette_16x12.bmp")));
        private ImageBrush _palettesTextureBrush;
        public ImageBrush PalettesTexture
        {
            get
            {
                if (_palettesTextureBrush == null)
                {
                    _palettesTextureBrush = new ImageBrush(_palettesTexture);
                    RefreshPaletteTexture();
                }
                return _palettesTextureBrush;
            }
        }

        #endregion

        #endregion

        #region Constructor

        private PropertyDependencyHandler _phHandler;
        public TilesetModel(LazyReference<Tileset> tileset, BlockEditorModel parentEditorModel) : base(tileset)
        {
            _graphicPath = tileset.Data.Graphic;
            _compressed = tileset.Data.Compressed;
            _secondary = tileset.Data.Secondary;
            _field2 = tileset.Data.Field2;
            _field3 = tileset.Data.Field3;
            _palettes = new ObservableCollectionEx<DisplayTuple<int, PaletteModel>>(tileset.Data.Palettes.Select((p, pi) =>
                new DisplayTuple<int, PaletteModel>(pi, new PaletteModel(p))));
            _palettes.ItemPropertyChanged += Palettes_ItemPropertyChanged;
            _blocks = new ObservableCollection<TilesetEntryModel>(tileset.Data.Blocks.Select(p => new TilesetEntryModel(p)));
            _selectedPalette = _palettes[0];
            _blockEditorViewModel = parentEditorModel;
            _phHandler = new PropertyDependencyHandler(this);
        }

        #endregion

        #region Events

        private void Palettes_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Value") RefreshPaletteTexture();
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

        private static BitmapPalette greyPalette = new BitmapPalette(Enumerable.Range(0, 15).Select(p => Color.FromArgb((byte)((p << 4) + 8), 0, 0, 0)).ToList());
        private void PrepareForPaletteShader(ref WriteableBitmap bitmap)
        {
#if DEBUG
            Debug.WriteLine("TilesetModel: PrepareForPaletteShader");
#endif
            WriteableBitmap newbitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, greyPalette);
            uint bufferLen = (uint)(bitmap.PixelHeight * bitmap.PixelWidth * bitmap.Format.BitsPerPixel) / 8;
            newbitmap.Lock();
            bitmap.Lock();
            memcpy(newbitmap.BackBuffer, bitmap.BackBuffer, (UIntPtr)bufferLen);
            bitmap.Unlock();
            newbitmap.AddDirtyRect(new Int32Rect(0, 0, newbitmap.PixelWidth, newbitmap.PixelHeight));
            newbitmap.Unlock();
            bitmap = newbitmap;//new WriteableBitmap(new FormatConvertedBitmap(newbitmap, PixelFormats.Bgra32, null, 0));
        }

        private unsafe void ResizeToPOT(ref WriteableBitmap bitmap)
        {
#if DEBUG
            Debug.WriteLine("TilesetModel: ResizeToPOT");
#endif
            int v = Math.Max(bitmap.PixelWidth, bitmap.PixelHeight);
            v--; v |= v >> 1; v |= v >> 2; v |= v >> 4; v |= v >> 8; v |= v >> 16; v++;
            WriteableBitmap newbitmap = new WriteableBitmap(v, v, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette);
            int bufferLen = (bitmap.PixelHeight * bitmap.PixelWidth * bitmap.Format.BitsPerPixel) / 8;
            newbitmap.Lock();
            bitmap.Lock();
            byte[] data = new byte[bufferLen];
            fixed (byte* p = data) memcpy((IntPtr)p, bitmap.BackBuffer, (UIntPtr)bufferLen);
            newbitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), data, bitmap.BackBufferStride, 0, 0);
            bitmap.Unlock();
            newbitmap.AddDirtyRect(new Int32Rect(0, 0, v, v));
            newbitmap.Unlock();
            bitmap = newbitmap;
        }

        private unsafe void RefreshPaletteTexture()
        {
#if DEBUG
            Debug.WriteLine("TilesetModel: RefreshPaletteTexture");
#endif
            if (_palettesTexture == null) return;
            _palettesTexture.Lock();
            byte* data = (byte*)_palettesTexture.BackBuffer;
            for (int i = 0; i < 6; i++)
            {
                int rowOffset = i * _palettesTexture.BackBufferStride;
                for (int j = 0; j < 16; j++)
                {
                    data[rowOffset + (j * 4)] = (byte)(_palettes[i].Value.Colors[j].Blue << 3);
                    data[rowOffset + (j * 4) + 1] = (byte)(_palettes[i].Value.Colors[j].Green << 3);
                    data[rowOffset + (j * 4) + 2] = (byte)(_palettes[i].Value.Colors[j].Red << 3);
                }
            }
            DisplayTuple<string, TilesetModel> addTileset = AdditionalDesignerTileset;
            if(addTileset != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    int rowOffset = (i + 6) * _palettesTexture.BackBufferStride;
                    for (int j = 0; j < 16; j++)
                    {
                        data[rowOffset + (j * 4)] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Blue << 3);
                        data[rowOffset + (j * 4) + 1] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Green << 3);
                        data[rowOffset + (j * 4) + 2] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Red << 3);
                    }
                }
            }
            _palettesTexture.AddDirtyRect(new Int32Rect(0, 0, 16, 12));
            _palettesTexture.Unlock();
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
