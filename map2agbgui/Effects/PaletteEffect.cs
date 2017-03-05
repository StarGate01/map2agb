using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace map2agbgui.Effects
{

    public class PaletteEffect : ShaderEffect
    {

        #region Private fields

        private static PixelShader _pixelShader = new PixelShader();
        private static Uri _bytecodePath = new Uri("pack://application:,,,/map2agbgui;component/Effects/PaletteEffect.ps");

        #endregion

        #region Properties

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(PaletteEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty PaletteProperty = 
            ShaderEffect.RegisterPixelShaderSamplerProperty("Palette", typeof(PaletteEffect), 1);
        public Brush Palette
        {
            get { return (Brush)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        #endregion

        #region Constructors

        static PaletteEffect()
        {
            _pixelShader.UriSource = _bytecodePath;
        }

        public PaletteEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(PaletteProperty);
        }

        #endregion

    }

}
