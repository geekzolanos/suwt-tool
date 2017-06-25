using System;
using System.Globalization;
using System.Windows.Data;

namespace suwt.Converters
{
    class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool req = Boolean.Parse(value.ToString());
            return !req;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
