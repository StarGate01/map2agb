using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace map2agbgui.Models.Main
{

    [ValueConversion(typeof(MapEntryType), typeof(FontStyle))]
    public class MapEntryTypeToFontStyleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((MapEntryType)value == MapEntryType.Nullpointer) return FontStyles.Italic;
            return FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
