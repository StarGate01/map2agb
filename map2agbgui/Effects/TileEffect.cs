using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace map2agbgui.Effects
{

    public class TileEffect : ShaderEffect
    {

        #region Private fields

        private static PixelShader _pixelShader = new PixelShader();
        private static Uri _bytecodePath = new Uri("pack://application:,,,/map2agbgui;component/Effects/PaletteEffect.ps");

        #endregion

        #region Properties

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(TileEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty OverlayProperty =
           ShaderEffect.RegisterPixelShaderSamplerProperty("Overlay", typeof(TileEffect), 1);
        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        public static readonly DependencyProperty PaletteProperty = 
            ShaderEffect.RegisterPixelShaderSamplerProperty("Palette", typeof(TileEffect), 2);
        public Brush Palette
        {
            get { return (Brush)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        public static readonly DependencyProperty OverlayPaletteProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("OverlayPalette", typeof(TileEffect), 3);
        public Brush OverlayPalette
        {
            get { return (Brush)GetValue(OverlayPaletteProperty); }
            set { SetValue(OverlayPaletteProperty, value); }
        }

        public static readonly DependencyProperty PaletteIndexProperty =
            DependencyProperty.Register("PaletteIndex", typeof(float), typeof(TileEffect),
            new UIPropertyMetadata(0f, PixelShaderConstantCallback(0)));
        public float PaletteIndex
        {
            get { return (float)GetValue(PaletteIndexProperty); }
            set { SetValue(PaletteIndexProperty, value); }
        }

        public static readonly DependencyProperty OverlayPaletteIndexProperty =
           DependencyProperty.Register("OverlayPaletteIndex", typeof(float), typeof(TileEffect),
           new UIPropertyMetadata(0f, PixelShaderConstantCallback(1)));
        public float OverlayPaletteIndex
        {
            get { return (float)GetValue(OverlayPaletteIndexProperty); }
            set { SetValue(OverlayPaletteIndexProperty, value); }
        }

        public static readonly DependencyProperty HFlipProperty =
         DependencyProperty.Register("HFlip", typeof(float), typeof(TileEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(2)));
        public float HFlip
        {
            get { return (float)GetValue(HFlipProperty); }
            set { SetValue(HFlipProperty, value); }
        }

        public static readonly DependencyProperty VFlipProperty =
         DependencyProperty.Register("VFlip", typeof(float), typeof(TileEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(3)));
        public float VFlip
        {
            get { return (float)GetValue(VFlipProperty); }
            set { SetValue(VFlipProperty, value); }
        }

        public static readonly DependencyProperty OverlayHFlipProperty =
        DependencyProperty.Register("OverlayHFlip", typeof(float), typeof(TileEffect),
        new UIPropertyMetadata(0f, PixelShaderConstantCallback(4)));
        public float OverlayHFlip
        {
            get { return (float)GetValue(OverlayHFlipProperty); }
            set { SetValue(OverlayHFlipProperty, value); }
        }

        public static readonly DependencyProperty OverlayVFlipProperty =
         DependencyProperty.Register("OverlayVFlip", typeof(float), typeof(TileEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(5)));
        public float OverlayVFlip
        {
            get { return (float)GetValue(OverlayVFlipProperty); }
            set { SetValue(OverlayVFlipProperty, value); }
        }

        #endregion

        #region Constructors

        static TileEffect()
        {
            _pixelShader.UriSource = _bytecodePath;
        }

        public TileEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(OverlayProperty);
            UpdateShaderValue(PaletteProperty);
            UpdateShaderValue(OverlayPaletteProperty);
            UpdateShaderValue(PaletteIndexProperty);
            UpdateShaderValue(OverlayPaletteIndexProperty);
            UpdateShaderValue(HFlipProperty);
            UpdateShaderValue(VFlipProperty);
            UpdateShaderValue(OverlayHFlipProperty);
            UpdateShaderValue(OverlayVFlipProperty);
        }

        #endregion

    }

}
