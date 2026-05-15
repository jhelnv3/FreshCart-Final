using FreshCart.Models;
using System.Globalization;

namespace FreshCart.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                return status switch
                {
                    OrderStatus.Pending => Color.FromArgb("#FF9800"),     // Orange
                    OrderStatus.Packing => Color.FromArgb("#2196F3"),     // Blue
                    OrderStatus.ToDeliver => Color.FromArgb("#9C27B0"),   // Purple
                    OrderStatus.ToReceive => Color.FromArgb("#FF5722"),   // Deep Orange
                    OrderStatus.Delivered => Color.FromArgb("#4CAF50"),   // Green
                    OrderStatus.Received => Color.FromArgb("#009688"),    // Teal
                    _ => Color.FromArgb("#757575")                        // Grey
                };
            }
            return Color.FromArgb("#757575");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}