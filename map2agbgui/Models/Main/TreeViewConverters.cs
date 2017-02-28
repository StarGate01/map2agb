using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using map2agbgui.Models.Main.Maps;

namespace map2agbgui.Models.Main
{

    [ValueConversion(typeof(DisplayTuple<int, ITupleFormattable>), typeof(bool))]
    public class TreeViewSelectedItemToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            else return value.GetType() == typeof(DisplayTuple<int, IMapModel>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

    [ValueConversion(typeof(DisplayTuple<int, ITupleFormattable>), typeof(MapHeaderModel))]
    public class TreeViewSelectedItemToMapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            else if (value.GetType() == typeof(DisplayTuple<int, IMapModel>))
            {
                IMapModel model = ((DisplayTuple<int, IMapModel>)value).Value;
                if (model.EntryMode == MapEntryType.Map) return (MapHeaderModel)model;
                else return null;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
