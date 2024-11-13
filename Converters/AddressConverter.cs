using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfNed.Converters
{
    public class AddressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string street = values[0]?.ToString();
            string building = values[1]?.ToString();
            string number = values[2]?.ToString();

            if (string.IsNullOrEmpty(number))
            {
                return $"ул. {street}, д. {building}";
            }
            else
            {
                return $"ул. {street}, д. {building}, кв. {number}";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
