using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace map2agbgui.Common
{

    [ValueConversion(typeof(SolidColorBrush), typeof(SolidColorBrush))]
    class ColorSingleChannelFilterConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return value;
            Color oldColor = ((SolidColorBrush)value).Color;
            switch (parameter.ToString())
            {
                case "R":
                    return new SolidColorBrush(Color.FromArgb(oldColor.R, 255, 0, 0));
                case "G":
                    return new SolidColorBrush(Color.FromArgb(oldColor.G, 0, 255, 0));
                case "B":
                    return new SolidColorBrush(Color.FromArgb(oldColor.B, 0, 0, 255));
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
