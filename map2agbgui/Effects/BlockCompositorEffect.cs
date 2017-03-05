using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace map2agbgui.Effects
{

    public class BlockCompositorEffect : ShaderEffect
    {

        #region Private fields

        private static PixelShader _pixelShader = new PixelShader();
        private static Uri _bytecodePath = new Uri("pack://application:,,,/map2agbgui;component/Effects/BlockCompositorEffect.ps");

        #endregion

        #region Properties

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BlockCompositorEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty LowGraphicProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("LowGraphic", typeof(BlockCompositorEffect), 1);
        public Brush LowGraphic
        {
            get { return (Brush)GetValue(LowGraphicProperty); }
            set { SetValue(LowGraphicProperty, value); }
        }

        public static readonly DependencyProperty LowGraphicWidthProperty =
         DependencyProperty.Register("LowGraphicWidth", typeof(float), typeof(BlockCompositorEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(9)));
        public float LowGraphicWidth
        {
            get { return (float)GetValue(LowGraphicWidthProperty); }
            set { SetValue(LowGraphicWidthProperty, value); }
        }

        public static readonly DependencyProperty LowGraphicHeightProperty =
          DependencyProperty.Register("LowGraphicHeight", typeof(float), typeof(BlockCompositorEffect),
          new UIPropertyMetadata(0f, PixelShaderConstantCallback(10)));
        public float LowGraphicHeight
        {
            get { return (float)GetValue(LowGraphicHeightProperty); }
            set { SetValue(LowGraphicHeightProperty, value); }
        }

        public static readonly DependencyProperty HighGraphicProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("HighGraphic", typeof(BlockCompositorEffect), 2);
        public Brush HighGraphic
        {
            get { return (Brush)GetValue(HighGraphicProperty); }
            set { SetValue(HighGraphicProperty, value); }
        }

        public static readonly DependencyProperty HighGraphicWidthProperty =
         DependencyProperty.Register("HighGraphicWidth", typeof(float), typeof(BlockCompositorEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(11)));
        public float HighGraphicWidth
        {
            get { return (float)GetValue(HighGraphicWidthProperty); }
            set { SetValue(HighGraphicWidthProperty, value); }
        }

        public static readonly DependencyProperty HighGraphicHeightProperty =
          DependencyProperty.Register("HighGraphicHeight", typeof(float), typeof(BlockCompositorEffect),
          new UIPropertyMetadata(0f, PixelShaderConstantCallback(12)));
        public float HighGraphicHeight
        {
            get { return (float)GetValue(HighGraphicHeightProperty); }
            set { SetValue(HighGraphicHeightProperty, value); }
        }


        #region Palettes

        public static readonly DependencyProperty MergedPaletteProperty =
              ShaderEffect.RegisterPixelShaderSamplerProperty("MergedPalette", typeof(BlockCompositorEffect), 3);
        public Brush MergedPalette
        {
            get { return (Brush)GetValue(MergedPaletteProperty); }
            set { SetValue(MergedPaletteProperty, value); }
        }


        public static readonly DependencyProperty SecondaryProperty =
         DependencyProperty.Register("Secondary", typeof(float), typeof(BlockCompositorEffect),
         new UIPropertyMetadata(0f, PixelShaderConstantCallback(8)));
        public float Secondary
        {
            get { return (float)GetValue(SecondaryProperty); }
            set { SetValue(SecondaryProperty, value); }
        }

        #endregion

        #region Tiles

        #region Bottom layer

        public static readonly DependencyProperty BottomLayer_TopLeftProperty =
          DependencyProperty.Register("BottomLayer_TopLeft", typeof(Point4D), typeof(BlockCompositorEffect),
          new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(0)));
        public Point4D BottomLayer_TopLeft
        {
            get { return (Point4D)GetValue(BottomLayer_TopLeftProperty); }
            set { SetValue(BottomLayer_TopLeftProperty, value); }
        }

        public static readonly DependencyProperty BottomLayer_TopRightProperty =
        DependencyProperty.Register("BottomLayer_TopRight", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(1)));
        public Point4D BottomLayer_TopRight
        {
            get { return (Point4D)GetValue(BottomLayer_TopRightProperty); }
            set { SetValue(BottomLayer_TopRightProperty, value); }
        }

        public static readonly DependencyProperty BottomLayer_BottomLeftProperty =
        DependencyProperty.Register("BottomLayer_BottomLeft", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(2)));
        public Point4D BottomLayer_BottomLeft
        {
            get { return (Point4D)GetValue(BottomLayer_BottomLeftProperty); }
            set { SetValue(BottomLayer_BottomLeftProperty, value); }
        }

        public static readonly DependencyProperty BottomLayer_BottomRightProperty =
        DependencyProperty.Register("BottomLayer_BottomRight", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(3)));
        public Point4D BottomLayer_BottomRight
        {
            get { return (Point4D)GetValue(BottomLayer_BottomRightProperty); }
            set { SetValue(BottomLayer_BottomRightProperty, value); }
        }

        #endregion

        #region Top layer

        public static readonly DependencyProperty TopLayer_TopLeftProperty =
          DependencyProperty.Register("TopLayer_TopLeft", typeof(Point4D), typeof(BlockCompositorEffect),
          new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(4)));
        public Point4D TopLayer_TopLeft
        {
            get { return (Point4D)GetValue(TopLayer_TopLeftProperty); }
            set { SetValue(TopLayer_TopLeftProperty, value); }
        }

        public static readonly DependencyProperty TopLayer_TopRightProperty =
        DependencyProperty.Register("TopLayer_TopRight", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(5)));
        public Point4D TopLayer_TopRight
        {
            get { return (Point4D)GetValue(TopLayer_TopRightProperty); }
            set { SetValue(TopLayer_TopRightProperty, value); }
        }

        public static readonly DependencyProperty TopLayer_BottomLeftProperty =
        DependencyProperty.Register("TopLayer_BottomLeft", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(6)));
        public Point4D TopLayer_BottomLeft
        {
            get { return (Point4D)GetValue(TopLayer_BottomLeftProperty); }
            set { SetValue(TopLayer_BottomLeftProperty, value); }
        }

        public static readonly DependencyProperty TopLayer_BottomRightProperty =
        DependencyProperty.Register("TopLayer_BottomRight", typeof(Point4D), typeof(BlockCompositorEffect),
        new UIPropertyMetadata(new Point4D(), PixelShaderConstantCallback(7)));
        public Point4D TopLayer_BottomRight
        {
            get { return (Point4D)GetValue(TopLayer_BottomRightProperty); }
            set { SetValue(TopLayer_BottomRightProperty, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        static BlockCompositorEffect()
        {
            _pixelShader.UriSource = _bytecodePath;
        }

        public BlockCompositorEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(MergedPaletteProperty);
            UpdateShaderValue(BottomLayer_TopLeftProperty);
            UpdateShaderValue(BottomLayer_TopRightProperty);
            UpdateShaderValue(BottomLayer_BottomLeftProperty);
            UpdateShaderValue(BottomLayer_BottomRightProperty);
            UpdateShaderValue(TopLayer_TopLeftProperty);
            UpdateShaderValue(TopLayer_TopRightProperty);
            UpdateShaderValue(TopLayer_BottomLeftProperty);
            UpdateShaderValue(TopLayer_BottomRightProperty);
            UpdateShaderValue(SecondaryProperty);
            UpdateShaderValue(LowGraphicProperty);
            UpdateShaderValue(HighGraphicProperty);
            UpdateShaderValue(LowGraphicWidthProperty);
            UpdateShaderValue(LowGraphicHeightProperty);
            UpdateShaderValue(HighGraphicWidthProperty);
            UpdateShaderValue(HighGraphicHeightProperty);
        }

        #endregion

    }

}
