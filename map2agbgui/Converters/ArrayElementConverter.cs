using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace map2agbgui.Converters
{

    [ValueConversion(typeof(object[]), typeof(ImageSource))]
    class GraphicTilesElementConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ((ImageBrush)((object[])(value[1]))[System.Convert.ToInt32(value[0])]).ImageSource;
            }
            catch (Exception) { return null; }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

    [ValueConversion(typeof(object[]), typeof(ImageSource))]
    class GraphicTilesTextureElementConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ((object[])(value[1]))[System.Convert.ToInt32(value[0])];
            }
            catch (Exception) { return null; }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
