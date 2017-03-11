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
using System.Windows.Threading;
using map2agbgui.Effects;

namespace map2agbgui.Models.BlockEditor
{

    public class TilesetModel : IRomSerializable<TilesetModel, LazyReference<Tileset>>, ITupleFormattable, IRaisePropertyChanged, IDisposable
    {

        //private static BitmapPalette greyPalette = new BitmapPalette(Enumerable.Range(0, 15).Select(p => Color.FromArgb((byte)((p << 4) + 8), 0, 0, 0)).ToList());

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
        [PropertyDependency(new string[] { "ValidAdditionalDesignerTileset", "AdditionalDesignerTileset"})] // "PalettesTexture" 
        public string AdditionalDesignerTilesetID
        {
            get
            {
                return _additionalDesignerTilesetID;
            }
            set
            {
                _additionalDesignerTilesetID = value;
                //_palettesTextureBrush = null;
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
        [PropertyDependency("Blocks")] //(new string[] { "Blocks", "PalettesTexture" }
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
                    _blocks = new ObservableCollectionEx<TilesetEntryModel>(_blocks.Take(targetSize));
                }
                else if(_blocks.Count < targetSize)
                {
                    while (_blocks.Count < targetSize) _blocks.Add(new TilesetEntryModel(new TilesetEntry(), this));
                }
                foreach (TilesetEntryModel block in _blocks) block.Dirty = true;
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
        //[PropertyDependency("PalettesTexture")]
        public ObservableCollectionEx<DisplayTuple<int, PaletteModel>> Palettes
        {
            get
            {
                return _palettes;
            }
            set
            {
                _palettes = value;
                //_palettesTextureBrush = null;
                _palettes.ItemPropertyChanged += Palettes_ItemPropertyChanged;
                foreach (TilesetEntryModel block in _blocks) block.Dirty = true;
                RaisePropertyChanged("Palettes");
            }
        }

        //private WriteableBitmap _palettesTexture = new WriteableBitmap(16, 12, 96, 96, PixelFormats.Bgr32, null);
        //private ImageBrush _palettesTextureBrush;
        //public ImageBrush PalettesTexture
        //{
        //    get
        //    {
        //        if (_palettesTextureBrush == null)
        //        {
        //            _palettesTextureBrush = new ImageBrush(_palettesTexture);
        //            RefreshPaletteTexture(_palettesTexture);
        //        }
        //        return _palettesTextureBrush;
        //    }
        //}

        #endregion

        #region Graphics and Blocks

        private string _graphicPath;
        [PropertyDependency(new string[] { "ValidImage", "Graphic"})] //, "GraphicTiles" 
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
                //_graphicTileBuffers = null;
                RaisePropertyChanged("GraphicPath");
            }
        }

        private WriteableBitmap _graphicBuffer;
        public WriteableBitmap Graphic
        {
            get
            {
                if (ValidImage)
                {
                    if (_graphicBuffer == null)
                    {
                        _graphicBuffer = new WriteableBitmap(new BitmapImage(new Uri(_graphicPath, UriKind.Absolute)));
                        _graphicBuffer.Freeze();
                        //ChangePalette(ref _graphicBuffer, greyPalette);
                        foreach (TilesetEntryModel block in _blocks) block.Dirty = true;
                    }
                    return _graphicBuffer;
                }
                else return null;
            }
        }

        //private ImageBrush[] _graphicTileBuffers;
        //public ImageBrush[] GraphicTiles
        //{
        //    get
        //    {
        //        if (_graphicTileBuffers == null)
        //        {
        //            _graphicTileBuffers = RefreshGraphicTiles((WriteableBitmap)Graphic);
        //        }
        //        return _graphicTileBuffers;
        //    }
        //}

        private ObservableCollectionEx<TilesetEntryModel> _blocks;
        public ObservableCollectionEx<TilesetEntryModel> Blocks
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
       
        private Thread backroundRendererTask;

        private bool _isRendering;
        public bool IsRendering
        {
            get
            {
                return _isRendering;
            }
            set
            {
                _isRendering = value;
                RaisePropertyChanged("IsRendering");
            }
        }

        private int _renderProgress;
        public int RenderProgress
        {
            get
            {
                return _renderProgress;
            }
            set
            {
                _renderProgress = value;
                RaisePropertyChanged("RenderProgress");
            }
        }

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
            _blocks = new ObservableCollectionEx<TilesetEntryModel>(tileset.Data.Blocks.Select(p => new TilesetEntryModel(p, this)));
            _selectedPalette = _palettes[0];
            _blockEditorViewModel = parentEditorModel;
            _phHandler = new PropertyDependencyHandler(this);
        }

        #endregion

        #region Events

        private void Palettes_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                //RefreshPaletteTexture(_palettesTexture);
                foreach (TilesetEntryModel block in _blocks) block.Dirty = true;
            }
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

//        private unsafe void RefreshPaletteTexture(WriteableBitmap bitmap)
//        {
//#if DEBUG
//            Debug.WriteLine("TilesetModel: RefreshPaletteTexture");
//#endif
//            if (bitmap == null) return;
//            bitmap.Lock();
//            byte* data = (byte*)bitmap.BackBuffer;
//            for (int i = 0; i < 6; i++)
//            {
//                int rowOffset = i * bitmap.BackBufferStride;
//                for (int j = 0; j < 16; j++)
//                {
//                    data[rowOffset + (j << 2)] = (byte)(_palettes[i].Value.Colors[j].Blue << 3);
//                    data[rowOffset + (j << 2) + 1] = (byte)(_palettes[i].Value.Colors[j].Green << 3);
//                    data[rowOffset + (j << 2) + 2] = (byte)(_palettes[i].Value.Colors[j].Red << 3);
//                }
//            }
//            DisplayTuple<string, TilesetModel> addTileset = AdditionalDesignerTileset;
//            if (addTileset != null)
//            {
//                for (int i = 0; i < 6; i++)
//                {
//                    int rowOffset = (i + 6) * bitmap.BackBufferStride;
//                    for (int j = 0; j < 16; j++)
//                    {
//                        data[rowOffset + (j * 4)] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Blue << 3);
//                        data[rowOffset + (j * 4) + 1] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Green << 3);
//                        data[rowOffset + (j * 4) + 2] = (byte)(addTileset.Value._palettes[i].Value.Colors[j].Red << 3);
//                    }
//                }
//            }
//            bitmap.AddDirtyRect(new Int32Rect(0, 0, 16, 12));
//            bitmap.Unlock();
//        }

        public void Dispose()
        {
            if (backroundRendererTask != null) backroundRendererTask.Abort();
        }

        #endregion

        #region Threads

        public void EnsureBlockRendererRunning()
        {
            if (backroundRendererTask == null || backroundRendererTask.ThreadState == System.Threading.ThreadState.Stopped)
            {
                backroundRendererTask = new Thread(BackgroundBlockRenderer);
                backroundRendererTask.SetApartmentState(ApartmentState.STA);
                backroundRendererTask.Name = "BackroundRendererTask";
                backroundRendererTask.Start();
            }
        }

        private unsafe void BackgroundBlockRenderer()
        {
#if DEBUG
            Debug.WriteLine("BackgroundBlockRenderer: Started");
#endif
            IsRendering = true;
            try
            {
                while (true)
                {
                    TilesetEntryModel dirtyBlock = null;
                    try
                    {
                        dirtyBlock = _blocks.First(p => p.Dirty);
                    }
                    catch (InvalidOperationException)
                    {
                        break;
                    }
                    RenderProgress = 100 - (int)((100f / (float)_blocks.Count) * (float)_blocks.Count(p => p.Dirty));

                    //targets for tile copies
                    WriteableBitmap newBlockImage = new WriteableBitmap(16, 16, 96, 96, PixelFormats.Bgra32, null);
                    WriteableBitmap newBlockImageOverlay = new WriteableBitmap(16, 16, 96, 96, PixelFormats.Bgra32, null);
                    newBlockImage.Lock();
                    byte* blockData = (byte*)newBlockImage.BackBuffer;
                    newBlockImageOverlay.Lock();
                    byte* blockDataOverlay = (byte*)newBlockImageOverlay.BackBuffer;

                    //copy tiles
                    for (int k = 0; k < 5; k += 4) for (int i = 0; i < 4; i++)
                        {
                            //TODO hflip and vflip
                            Int32Rect pos = new Int32Rect((i == 1 || i == 3) ? 8 : 0, (i == 2 || i == 3) ? 8 : 0, 8, 8);
                            WriteableBitmap tile = GetTile(dirtyBlock.Tilemap[i + k].TileID);
                            IList<Color> currentPalette = _palettes[dirtyBlock.Tilemap[i + k].PalIndex].Value.Colors.Select(p => p.Color).ToList();
                            currentPalette[0] = Color.FromArgb(0, 0, 0, 0); //color 0 = transparent
                            ChangePalette(ref tile, new BitmapPalette(currentPalette));
                            WriteableBitmap fbitmap = new WriteableBitmap(new FormatConvertedBitmap(tile, PixelFormats.Bgra32, null, 0));
                            fbitmap.Lock();
                            byte* fbData = (byte*)fbitmap.BackBuffer;
                            ((k == 0) ? newBlockImage : newBlockImageOverlay).WritePixels(pos, (IntPtr)fbData, 256, 32);
                            fbitmap.Unlock();
                        }

                    newBlockImageOverlay.AddDirtyRect(new Int32Rect(0, 0, 16, 16));
                    newBlockImageOverlay.Unlock();
                    newBlockImage.AddDirtyRect(new Int32Rect(0, 0, 16, 16));
                    newBlockImage.Unlock();

                    //merge targets
                    AddTransparentBlock(newBlockImage, newBlockImageOverlay);
                    newBlockImage.Freeze();

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        dirtyBlock.Graphic = newBlockImage;
                        dirtyBlock.Dirty = false;
                    }));
                }
            }
            catch (ThreadAbortException)
            {
#if DEBUG
                Debug.WriteLine("BackgroundBlockRenderer: Aborted");
#endif
            }
#if DEBUG
            Debug.WriteLine("BackgroundBlockRenderer: Ended");
#endif
            IsRendering = false;
        }

        #endregion

        #region Imaging methods

        #region Native

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        #endregion

        private void ChangePalette(ref WriteableBitmap bitmap, BitmapPalette palette)
        {
            if (bitmap == null) return;
            WriteableBitmap newbitmap = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, palette);
            int bufferLen = (bitmap.PixelHeight * bitmap.PixelWidth * bitmap.Format.BitsPerPixel) >> 3;
            newbitmap.Lock();
            bitmap.Lock();
            memcpy(newbitmap.BackBuffer, bitmap.BackBuffer, (UIntPtr)bufferLen);
            bitmap.Unlock();
            newbitmap.AddDirtyRect(new Int32Rect(0, 0, newbitmap.PixelWidth, newbitmap.PixelHeight));
            newbitmap.Unlock();
            newbitmap.Freeze();
            bitmap = newbitmap;
        }

        private unsafe WriteableBitmap GetTile(int tileID)
        {
            WriteableBitmap tile = new WriteableBitmap(8, 8, 96, 96, PixelFormats.Indexed4, _graphicBuffer.Palette);
            int cols = _graphicBuffer.PixelWidth >> 3;
            int yoffset = tileID / cols;
            int xoffset = tileID - (yoffset * cols);
            tile.Lock();
            _graphicBuffer.CopyPixels(new Int32Rect(xoffset << 3, yoffset << 3, 8, 8), tile.BackBuffer, 32, 4);
            tile.AddDirtyRect(new Int32Rect(0, 0, 8, 8));
            tile.Unlock();
            return tile;
        }

        private unsafe void AddTransparentBlock(WriteableBitmap target, WriteableBitmap overlay)
        {
            target.Lock();
            byte* targetData = (byte*)target.BackBuffer;
            overlay.Lock();
            byte* overlayData = (byte*)overlay.BackBuffer;
            for(int i=0; i<1024; i+=4)
            {
                if (overlayData[i + 3] != 0)
                {
                    targetData[i] = overlayData[i];
                    targetData[i + 1] = overlayData[i + 1];
                    targetData[i + 2] = overlayData[i + 2];
                    targetData[i + 3] = overlayData[i + 3];
                }
            }
            overlay.Unlock();
            target.AddDirtyRect(new Int32Rect(0, 0, 16, 16));
            target.Unlock();
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
