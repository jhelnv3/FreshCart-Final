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
    public class CartViewModel : BindableObject
    {
        public ObservableCollection<CartItem> CartItems => DataService.Cart;

        public decimal TotalAmount
        {
            get => CartItems.Sum(i => i.TotalPrice);
        }

        public ICommand PurchaseCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand LogoutCommand { get; }

        public CartViewModel()
        {
            PurchaseCommand = new Command(OnPurchase);
            RemoveItemCommand = new Command<CartItem>(OnRemoveItem);
            LogoutCommand = new Command(async () => await OnLogout());
        }

        public void RefreshTotal()
        {
            OnPropertyChanged(nameof(TotalAmount));
        }

        private async void OnPurchase()
        {
            if (!CartItems.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Empty Cart", "Your cart is empty", "OK");
                return;
            }

            string paymentMethod = await Application.Current.MainPage.DisplayActionSheet(
                "Select Payment Method",
                "Cancel",
                null,
                "Cash on Delivery",
                "Online Payment");

            if (paymentMethod == "Cancel" || string.IsNullOrEmpty(paymentMethod))
                return;

            var orderNumber = DataService.PlaceOrder(paymentMethod);
            RefreshTotal();
            await Application.Current.MainPage.DisplayAlert("Order Placed",
                $"Order {orderNumber} has been placed successfully!\nPayment: {paymentMethod}", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private void OnRemoveItem(CartItem item)
        {
            // Find the product in DataService and restore stock
            var product = DataService.Products.FirstOrDefault(p => p.Id == item.Product.Id);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
            }

            DataService.Cart.Remove(item);
            RefreshTotal();

            // Notify that cart was updated
            MessagingCenter.Send<CartViewModel>(this, "CartUpdated");
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