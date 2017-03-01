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
            try
            {
                string strval = value.ToString();
                int intval = 0;
                if (strval.ToLower().StartsWith("0x"))
                {
                    strval = strval.Substring(2);
                    intval = int.Parse(strval, NumberStyles.HexNumber);
                }
                else intval = int.Parse(strval, NumberStyles.Integer);
                if (intval < byte.MinValue) intval = byte.MinValue;
                if (intval > byte.MaxValue) intval = byte.MaxValue;
                if (parameter != null && intval > int.Parse(parameter.ToString())) intval = byte.Parse(parameter.ToString());
                return (byte)intval;
            }
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
            try
            {
                string strval = value.ToString();
                int intval = 0;
                if (strval.ToLower().StartsWith("0x"))
                {
                    strval = strval.Substring(2);
                    intval = int.Parse(strval, NumberStyles.HexNumber);
                }
                else intval = int.Parse(strval, NumberStyles.Integer);
                if (intval < ushort.MinValue) intval = ushort.MinValue;
                if (intval > ushort.MaxValue) intval = ushort.MaxValue;
                return (ushort)intval;
            }
            catch (Exception) { return 0; }
        }

    }

}
