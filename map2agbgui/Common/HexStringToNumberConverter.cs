using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace map2agbgui.Common
{

    [ValueConversion(typeof(string), typeof(byte))]
    public class ByteToHexStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte number = (byte)value;
            return string.Format("0x{0:X2}", number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try { return byte.Parse(value.ToString(), NumberStyles.HexNumber); }
            catch (Exception) { return 0; }
        }

    }

    [ValueConversion(typeof(string), typeof(ushort))]
    public class UShortToHexStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ushort number = (ushort)value;
            return string.Format("0x{0:X4}", number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try { return ushort.Parse(value.ToString(), NumberStyles.HexNumber); }
            catch (Exception) { return 0; }
        }

    }

}
