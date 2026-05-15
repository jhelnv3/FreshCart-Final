using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreshCart.Models;
using System.Globalization;

namespace FreshCart.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status && parameter is string targetStatus)
            {
                if (Enum.TryParse<OrderStatus>(targetStatus, out OrderStatus target))
                {
                    return status == target;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}