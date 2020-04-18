using System;
using System.Globalization;
using System.Windows.Data;
using RimWorldClothingManagerLibrary;

namespace RimWorldClothingManagerUi.Converters
{
    public class LayerCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = (Layers)value;

            return items != null ? $"Layers: {string.Join(", ", items.Li)}" : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
