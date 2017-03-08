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
using System.Collections.Concurrent;
using map2agbgui.Controls;
using System.Threading;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetModel : IRomSerializable<TilesetModel, LazyReference<Tileset>>, ITupleFormattable, IRaisePropertyChanged, IDisposable
    {

        private const string TESTPALETTE = "pack://application:,,,/map2agbgui;component/Assets/TestPalette_16x12.bmp";
        private const string TESTBLOCK = "pack://application:,,,/map2agbgui;component/Assets/TestBlock_16x16.bmp";

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

        #region General

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
        [PropertyDependency(new string[] { "Blocks", "PalettesTexture" })]
        public bool Secondary
        {
            get
            {
                return _secondary;
            }
            set
            {
                _secondary = value;
                int targetSize = _secondary ? Tileset.MAX_SECOND_TILESET_SIZE : Tileset.MAX_FIRST_TILESET_SIZE;
                if(_blocks.Count > targetSize)
                {
                    _blocks = new ObservableCollection<TilesetEntryModel>(_blocks.Take(targetSize));
                }
                else if(_blocks.Count < targetSize)
                {
                    while (_blocks.Count < targetSize) _blocks.Add(new TilesetEntryModel(new TilesetEntry(), this));
                }
                AdjustBlockLength();
                EnqueueAllBlocks();
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

        #endregion

        #region Palettes

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

        private WriteableBitmap _palettesTexture = new WriteableBitmap(new BitmapImage(new Uri(TESTPALETTE)));
        private ImageBrush _palettesTextureBrush;
        public ImageBrush PalettesTexture
        {
            get
            {
                if (_palettesTextureBrush == null)
                {
                    _palettesTextureBrush = new ImageBrush(_palettesTexture);
                    RefreshPaletteTexture(_palettesTexture);
                }
                return _palettesTextureBrush;
            }
        }

        #endregion

        #region Graphics and Blocks

        private string _graphicPath;
        [PropertyDependency(new string[] { "ValidImage", "Graphic", "GraphicTiles" })]
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
                _graphicTileBuffers = null;
                RaisePropertyChanged("GraphicPath");
            }
        }

        private WriteableBitmap _graphicBuffer;
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
                    return _graphicBuffer;
                }
                else return null;
            }
        }

        private ImageBrush[] _graphicTileBuffers;
        public ImageBrush[] GraphicTiles
        {
            get
            {
                if (_graphicTileBuffers == null)
                {
                    _graphicTileBuffers = RefreshGraphicTiles((WriteableBitmap)Graphic);
                }
                return _graphicTileBuffers;
            }
        }

        private ObservableCollection<TilesetEntryModel> _blocks;
        [PropertyDependency("RenderedBlocks")]
        public ObservableCollection<TilesetEntryModel> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                AdjustBlockLength();
                EnqueueAllBlocks();
                RaisePropertyChanged("Blocks");
            }
        }

        private ObservableCollection<BitmapSource> _renderedBlocks;
        public ObservableCollection<BitmapSource> RenderedBlocks
        {
            get
            {
                return _renderedBlocks;
            }
            set
            {
                _renderedBlocks = value;
                RaisePropertyChanged("RenderedBlocks");
            }
        }

        private object usingGraphicTileBuffer = new object();
        private ConcurrentQueue<int> renderBlockQueue;
        private Thread backroundRendererTask;

        #endregion

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
            _blocks = new ObservableCollection<TilesetEntryModel>(tileset.Data.Blocks.Select(p => new TilesetEntryModel(p, this)));
            _renderedBlocks = new ObservableCollection<BitmapSource>();
            _selectedPalette = _palettes[0];
            _blockEditorViewModel = parentEditorModel;
            _phHandler = new PropertyDependencyHandler(this);
            renderBlockQueue = new ConcurrentQueue<int>();
            AdjustBlockLength();
            backroundRendererTask = new Thread(BackgroundBlockRenderer);
            backroundRendererTask.SetApartmentState(ApartmentState.STA);
            backroundRendererTask.Start();
        }

        #endregion

        #region Events

        private void Palettes_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Value") RefreshPaletteTexture(_palettesTexture);
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
            if (bitmap == null) return;
            WriteableBitmap newbitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, greyPalette);
            int bufferLen = (bitmap.PixelHeight * bitmap.PixelWidth * bitmap.Format.BitsPerPixel) >> 3;
            newbitmap.Lock();
            bitmap.Lock();
            memcpy(newbitmap.BackBuffer, bitmap.BackBuffer, (UIntPtr)bufferLen);
            bitmap.Unlock();
            newbitmap.AddDirtyRect(new Int32Rect(0, 0, newbitmap.PixelWidth, newbitmap.PixelHeight));
            newbitmap.Unlock();
            bitmap = newbitmap;
        }

        private unsafe void RefreshPaletteTexture(WriteableBitmap bitmap)
        {
#if DEBUG
            Debug.WriteLine("TilesetModel: RefreshPaletteTexture");
#endif
            if (bitmap == null) return;
            bitmap.Lock();
            byte* data = (byte*)bitmap.BackBuffer;
            for (int i = 0; i < 6; i++)
            {
                int rowOffset = i * bitmap.BackBufferStride;
                for (int j = 0; j < 16; j++)
                {
                    data[rowOffset + (j << 2)] = (byte)(_palettes[i].Value.Colors[j].Blue << 3);
                    data[rowOffset + (j << 2) + 1] = (byte)(_palettes[i].Value.Colors[j].Green << 3);
                    data[rowOffset + (j << 2) + 2] = (byte)(_palettes[i].Value.Colors[j].Red << 3);
                }
            }
            DisplayTuple<string, TilesetModel> addTileset = AdditionalDesignerTileset;
            if (addTileset != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    int rowOffset = (i + 6) * bitmap.BackBufferStride;
                    for (int j = 0; j < 16; j++)
                    {
                        data[rowOffset + (j * 4)] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Blue << 3);
                        data[rowOffset + (j * 4) + 1] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Green << 3);
                        data[rowOffset + (j * 4) + 2] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Red << 3);
                    }
                }
            }
            bitmap.AddDirtyRect(new Int32Rect(0, 0, 16, 12));
            bitmap.Unlock();
        }

        private unsafe ImageBrush[] RefreshGraphicTiles(WriteableBitmap bitmap)
        {
#if DEBUG
            Debug.WriteLine("TilesetModel: RefreshGraphicTiles");
#endif
            if (bitmap == null) return new ImageBrush[0];
            int rows = bitmap.PixelHeight >> 3;
            int cols = bitmap.PixelWidth >> 3;
            WriteableBitmap[] tiles = new WriteableBitmap[rows * cols];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int offset = (cols * y) + x;
                    tiles[offset] = new WriteableBitmap(8, 8, bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette);
                    tiles[offset].Lock();
                    byte* tileData = (byte*)tiles[offset].BackBuffer;
                    bitmap.CopyPixels(new Int32Rect(x << 3, y << 3, 8, 8), (IntPtr)tileData, bitmap.Format.BitsPerPixel << 3, bitmap.Format.BitsPerPixel);
                    tiles[offset].AddDirtyRect(new Int32Rect(0, 0, 8, 8));
                    tiles[offset].Unlock();
                }
            }
            return tiles.Select(p => new ImageBrush(p)).ToArray();
        }

        #endregion

        #region Threads

        private void AdjustBlockLength()
        {
            if (_renderedBlocks.Count > _blocks.Count)
            {
                _renderedBlocks = new ObservableCollection<BitmapSource>(_renderedBlocks.Take(_blocks.Count));
            }
            else if (_blocks.Count < _blocks.Count)
            {
                while (_blocks.Count < _blocks.Count) _renderedBlocks.Add(new BitmapImage(new Uri(TESTBLOCK)));
            }
        }

        private void EnqueueBlocks(params int[] index)
        {
            for(int i=0; i<index.Length; i++) renderBlockQueue.Enqueue(index[i]);
        }

        private void EnqueueAllBlocks()
        {
            for (int i = 0; i < _blocks.Count; i++) renderBlockQueue.Enqueue(i);
        }

        private void BackgroundBlockRenderer()
        {
#if DEBUG
            Debug.WriteLine("BackgroundBlockRenderer: Started");
#endif
            BitmapImage noimage = new BitmapImage(new Uri(TESTBLOCK));
            Image img = new Image();
            img.BeginInit();
            img.Height = 16;
            img.Width = 16;
            img.EndInit();
            while (true)
            {
                if(!renderBlockQueue.IsEmpty && _graphicTileBuffers != null && _graphicTileBuffers.Length > 0)
                {
                    int index;
                    if(renderBlockQueue.TryDequeue(out index))
                    {
#if DEBUG
                        Debug.WriteLine("BackgroundBlockRenderer: Render tile #" + index);
#endif
                        img.Source = _graphicTileBuffers[0].ImageSource;
                        img.UpdateLayout();

                        RenderTargetBitmap bmp = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
                        bmp.Render(img);
                        RenderedBlocks[index] = bmp;

                        img.Source = null;
                        //render Blocks[index] into RenderedBlocks[index]
                    }
                }
            }
        }

        public void Dispose()
        {
            backroundRendererTask.Abort();
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
