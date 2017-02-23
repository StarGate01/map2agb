using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace map2agbgui.Models.Main
{

    [ValueConversion(typeof(NumericDisplayTuple<ITupleFormattable>), typeof(bool))]
    public class TreeViewSeletedItemToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            else return value.GetType() == typeof(NumericDisplayTuple<IMapModel>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

    [ValueConversion(typeof(NumericDisplayTuple<ITupleFormattable>), typeof(MapModel))]
    public class TreeViewSeletedItemToMapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            else if (value.GetType() == typeof(NumericDisplayTuple<IMapModel>))
            {
                IMapModel model = ((NumericDisplayTuple<IMapModel>)value).Value;
                if (model.GetType() == typeof(MapModel)) return (MapModel)model;
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
