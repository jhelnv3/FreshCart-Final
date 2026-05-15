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
    public class StaffViewModel : BindableObject
    {
        private string _newProductName;
        private string _newProductPrice;
        private string _newProductStock;
        private string _searchQuery;
        private ObservableCollection<Product> _filteredProducts;

        public ObservableCollection<Order> Orders => DataService.Orders;
        public ObservableCollection<Product> Products => DataService.Products;

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts ?? new ObservableCollection<Product>(Products);
            set { _filteredProducts = value; OnPropertyChanged(); }
        }

        public string NewProductName
        {
            get => _newProductName;
            set { _newProductName = value; OnPropertyChanged(); }
        }

        public string NewProductPrice
        {
            get => _newProductPrice;
            set { _newProductPrice = value; OnPropertyChanged(); }
        }

        public string NewProductStock
        {
            get => _newProductStock;
            set { _newProductStock = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilterProducts();
            }
        }

        public ICommand UpdateStatusCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SelectProductForUpdateCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand LogoutCommand { get; }

        public StaffViewModel()
        {
            UpdateStatusCommand = new Command<Order>(OnUpdateStatus);
            AddProductCommand = new Command(OnAddProduct);
            SearchCommand = new Command(FilterProducts);
            SelectProductForUpdateCommand = new Command<Product>(OnSelectProductForUpdate);
            DeleteProductCommand = new Command<Product>(OnDeleteProduct);
            LogoutCommand = new Command(async () => await OnLogout());

            FilteredProducts = new ObservableCollection<Product>(Products);
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredProducts = new ObservableCollection<Product>(Products);
            }
            else
            {
                var searchTerm = SearchQuery.ToLower();
                var filtered = Products.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Id.ToString().Contains(searchTerm) ||
                    p.Price.ToString("F2").Contains(searchTerm)
                ).ToList();
                FilteredProducts = new ObservableCollection<Product>(filtered);
            }
        }

        private async void OnSelectProductForUpdate(Product product)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                $"Update: {product.Name}",
                "Cancel",
                null,
                "Edit Name",
                "Edit Price",
                "Edit Stock",
                "Edit All Fields");

            if (action == "Cancel" || string.IsNullOrEmpty(action))
                return;

            switch (action)
            {
                case "Edit Name":
                    await UpdateProductName(product);
                    break;
                case "Edit Price":
                    await UpdateProductPrice(product);
                    break;
                case "Edit Stock":
                    await UpdateProductStock(product);
                    break;
                case "Edit All Fields":
                    await UpdateAllFields(product);
                    break;
            }

            FilterProducts();
            OnPropertyChanged(nameof(Products));
        }

        private async Task UpdateProductName(Product product)
        {
            string newName = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product Name",
                $"Current name: {product.Name}\nEnter new name:",
                "Update",
                "Cancel",
                product.Name);

            if (!string.IsNullOrWhiteSpace(newName) && newName != product.Name)
            {
                product.Name = newName;
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Product name updated to: {newName}", "OK");
            }
        }

        private async Task UpdateProductPrice(Product product)
        {
            string newPriceStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Price",
                $"Product: {product.Name}\nCurrent price: ${product.Price:F2}\nEnter new price:",
                "Update",
                "Cancel",
                product.Price.ToString("F2"),
                keyboard: Keyboard.Numeric);

            if (decimal.TryParse(newPriceStr, out decimal newPrice) && newPrice >= 0)
            {
                product.Price = newPrice;
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Price updated to: ${newPrice:F2}", "OK");
            }
        }

        private async Task UpdateProductStock(Product product)
        {
            string newStockStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Stock",
                $"Product: {product.Name}\nCurrent stock: {product.StockQuantity}\nEnter new stock:",
                "Update",
                "Cancel",
                product.StockQuantity.ToString(),
                keyboard: Keyboard.Numeric);

            if (int.TryParse(newStockStr, out int newStock) && newStock >= 0)
            {
                product.StockQuantity = newStock;
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Stock updated to: {newStock}", "OK");
            }
        }

        private async Task UpdateAllFields(Product product)
        {
            string newName = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 1/3",
                $"Current name: {product.Name}\nEnter new name:",
                "Next",
                "Cancel",
                product.Name);

            if (newName == null) return;

            string newPriceStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 2/3",
                $"Current price: ${product.Price:F2}\nEnter new price:",
                "Next",
                "Cancel",
                product.Price.ToString("F2"),
                keyboard: Keyboard.Numeric);

            if (newPriceStr == null) return;

            string newStockStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 3/3",
                $"Current stock: {product.StockQuantity}\nEnter new stock:",
                "Update",
                "Cancel",
                product.StockQuantity.ToString(),
                keyboard: Keyboard.Numeric);

            if (newStockStr == null) return;

            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;

            if (decimal.TryParse(newPriceStr, out decimal newPrice) && newPrice >= 0)
                product.Price = newPrice;

            if (int.TryParse(newStockStr, out int newStock) && newStock >= 0)
                product.StockQuantity = newStock;

            await Application.Current.MainPage.DisplayAlert("Success",
                "All product details updated successfully!", "OK");
        }

        private async void OnDeleteProduct(Product product)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Product",
                $"Are you sure you want to delete '{product.Name}'?\nThis action cannot be undone.",
                "Delete",
                "Cancel");

            if (confirm)
            {
                DataService.Products.Remove(product);
                FilterProducts();
                OnPropertyChanged(nameof(Products));
                await Application.Current.MainPage.DisplayAlert("Deleted",
                    $"{product.Name} has been deleted", "OK");
            }
        }

        private async void OnUpdateStatus(Order order)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                $"Update Status for {order.OrderNumber}\nCurrent Status: {order.Status}",
                "Cancel",
                null,
                "Packing",
                "To Deliver",
                "To Receive",
                "Delivered");

            if (action == "Cancel" || string.IsNullOrEmpty(action))
                return;

            OrderStatus newStatus = action switch
            {
                "Packing" => OrderStatus.Packing,
                "To Deliver" => OrderStatus.ToDeliver,
                "To Receive" => OrderStatus.ToReceive,
                "Delivered" => OrderStatus.Delivered,
                _ => order.Status
            };

            // Update the status - this now triggers PropertyChanged on the Order object
            DataService.UpdateOrderStatus(order.OrderNumber, newStatus);

            // Force refresh the UI
            OnPropertyChanged(nameof(Orders));

            // Additional refresh to ensure all bound properties update
            var index = DataService.Orders.IndexOf(order);
            if (index >= 0)
            {
                DataService.Orders[index] = order;
            }

            string message = newStatus == OrderStatus.Delivered
                ? $"Order {order.OrderNumber} has been delivered. Waiting for customer confirmation."
                : $"Order {order.OrderNumber} is now: {newStatus}";

            await Application.Current.MainPage.DisplayAlert("Status Updated", message, "OK");
        }

        private void OnAddProduct()
        {
            if (string.IsNullOrWhiteSpace(NewProductName) ||
                string.IsNullOrWhiteSpace(NewProductPrice) ||
                string.IsNullOrWhiteSpace(NewProductStock))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }

            if (!decimal.TryParse(NewProductPrice, out decimal price) || price < 0)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Invalid price", "OK");
                return;
            }

            if (!int.TryParse(NewProductStock, out int stock) || stock < 0)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Invalid stock quantity", "OK");
                return;
            }

            var newProduct = new Product
            {
                Id = DataService.Products.Any() ? DataService.Products.Max(p => p.Id) + 1 : 1,
                Name = NewProductName,
                Price = price,
                StockQuantity = stock
            };

            DataService.Products.Add(newProduct);
            FilterProducts();

            NewProductName = string.Empty;
            NewProductPrice = string.Empty;
            NewProductStock = string.Empty;

            Application.Current.MainPage.DisplayAlert("Success",
                $"Product '{newProduct.Name}' added successfully!", "OK");
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