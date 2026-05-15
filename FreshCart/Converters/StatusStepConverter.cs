using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreshCart.Models;
using System.Globalization;

namespace FreshCart.Converters
{
    public class StatusStepConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus currentStatus && parameter is string stepName)
            {
                bool isActive = false;
                bool isCompleted = false;

                switch (stepName)
                {
                    case "Pending":
                        isActive = currentStatus == OrderStatus.Pending;
                        isCompleted = currentStatus > OrderStatus.Pending;
                        break;
                    case "Packing":
                        isActive = currentStatus == OrderStatus.Packing;
                        isCompleted = currentStatus > OrderStatus.Packing;
                        break;
                    case "ToDeliver":
                        isActive = currentStatus == OrderStatus.ToDeliver;
                        isCompleted = currentStatus > OrderStatus.ToDeliver;
                        break;
                    case "ToReceive":
                        isActive = currentStatus == OrderStatus.ToReceive;
                        isCompleted = currentStatus > OrderStatus.ToReceive;
                        break;
                    case "Delivered":
                        isActive = currentStatus == OrderStatus.Delivered;
                        isCompleted = currentStatus > OrderStatus.Delivered;
                        break;
                    case "Received":
                        isActive = currentStatus == OrderStatus.Received;
                        isCompleted = false;
                        break;
                }

                if (isActive)
                    return Color.FromArgb("#2196F3"); // Blue for current step
                else if (isCompleted)
                    return Color.FromArgb("#4CAF50"); // Green for completed steps
                else
                    return Color.FromArgb("#BDBDBD"); // Grey for upcoming steps
            }
            return Color.FromArgb("#BDBDBD");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}