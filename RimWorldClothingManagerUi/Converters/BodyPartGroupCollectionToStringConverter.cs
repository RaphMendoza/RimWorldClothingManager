using System;
using System.Globalization;
using System.Windows.Data;
using RimWorldClothingManagerLibrary;

namespace RimWorldClothingManagerUi.Converters
{
    public class BodyPartGroupCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = (BodyPartGroups)value;

            return items != null ? $"Body Part Groups: {string.Join(", ", items.Li)}" : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}