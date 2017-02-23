using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Reflection;

namespace map2agbgui.Common
{

    public class IconMods
    {

        /// <summary>
        /// Converts a GDI icon to a WPF image source
        /// </summary>
        /// <param name="icon">The icon</param>
        /// <returns></returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

    }

    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class SystemIconToImageSourceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || (typeof(SystemIcons)).GetProperty(parameter.ToString()) == null) parameter = "WinLogo";
            return IconMods.ToImageSource((Icon)(typeof (SystemIcons)).GetProperty(parameter.ToString()).GetValue(null));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
