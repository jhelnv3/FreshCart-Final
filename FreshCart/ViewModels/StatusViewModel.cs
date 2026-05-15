using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreshCart.Models;
using FreshCart.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FreshCart.ViewModels
{
    public class StatusViewModel : BindableObject
    {
        public ObservableCollection<Order> Orders => DataService.Orders;
        public ICommand ConfirmReceiptCommand { get; }
        public ICommand LogoutCommand { get; }

        public StatusViewModel()
        {
            ConfirmReceiptCommand = new Command<Order>(OnConfirmReceipt);
            LogoutCommand = new Command(async () => await OnLogout());
        }

        public void RefreshOrders()
        {
            OnPropertyChanged(nameof(Orders));
        }

        private async void OnConfirmReceipt(Order order)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Receipt",
                $"Have you received all items in Order {order.OrderNumber}?\n\n" +
                $"Products: {order.Items.Count}\n" +
                $"Total Amount: ${order.TotalAmount:F2}\n\n" +
                "This action cannot be undone.",
                "Yes, I received it!",
                "Not yet");

            if (confirm)
            {
                DataService.UpdateOrderStatus(order.OrderNumber, OrderStatus.Received);
                RefreshOrders();

                // Force refresh by replacing order in collection
                var index = DataService.Orders.IndexOf(order);
                if (index >= 0)
                {
                    DataService.Orders[index] = order;
                }

                await Application.Current.MainPage.DisplayAlert(
                    "Order Received",
                    $"Thank you! Order {order.OrderNumber} has been marked as received.\n\n" +
                    "Enjoy your products! 🎉",
                    "OK");
            }
        }

        private async Task OnLogout()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Logout",
                "Are you sure you want to logout?",
                "Yes",
                "No");

            if (confirm)
            {
                DataService.CurrentUser = null;
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}