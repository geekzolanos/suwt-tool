using System;
using System.Globalization;
using System.Windows.Data;

namespace suwt.Converters
{
    class StatusValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool req = Boolean.Parse(value.ToString());
            return (req ? "Servicio Iniciado" : "Servicio Detenido");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
