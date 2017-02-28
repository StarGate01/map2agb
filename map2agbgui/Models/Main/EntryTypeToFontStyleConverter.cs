using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using map2agbgui.Models.Main.Maps;

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

    [ValueConversion(typeof(BankEntryType), typeof(FontStyle))]
    public class BankEntryTypeToFontStyleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((BankEntryType)value == BankEntryType.Nullpointer) return FontStyles.Italic;
            return FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
