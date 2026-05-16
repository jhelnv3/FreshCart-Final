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
        private string _newProductCategory;
        private string _searchQuery;
        private string _selectedCategory;
        private string _newCategoryName;
        private ObservableCollection<Product> _filteredProducts;
        private ObservableCollection<string> _categories;

        public ObservableCollection<Order> Orders => DataService.Orders;
        public ObservableCollection<Product> Products => DataService.Products;

        public decimal TotalRevenue => Orders.Sum(o => o.TotalAmount);

        public ObservableCollection<string> Categories
        {
            get => _categories ?? DataService.Categories;
            set { _categories = value; OnPropertyChanged(); }
        }

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

        public string NewProductCategory
        {
            get => _newProductCategory;
            set { _newProductCategory = value; OnPropertyChanged(); }
        }

        public string NewCategoryName
        {
            get => _newCategoryName;
            set { _newCategoryName = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); FilterProducts(); }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); FilterProducts(); }
        }

        public ICommand UpdateStatusCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SelectProductForUpdateCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand LogoutCommand { get; }

        public StaffViewModel()
        {
            UpdateStatusCommand = new Command<Order>(OnUpdateStatus);
            AddProductCommand = new Command(OnAddProduct);
            SearchCommand = new Command(FilterProducts);
            SelectProductForUpdateCommand = new Command<Product>(OnSelectProductForUpdate);
            DeleteProductCommand = new Command<Product>(OnDeleteProduct);
            ClearFilterCommand = new Command(() => { SelectedCategory = null; SearchQuery = string.Empty; });
            AddCategoryCommand = new Command(OnAddCategory);
            EditCategoryCommand = new Command<string>(OnEditCategory);
            DeleteCategoryCommand = new Command<string>(OnDeleteCategory);
            LogoutCommand = new Command(async () => await OnLogout());

            Categories = new ObservableCollection<string>(DataService.Categories);
            FilteredProducts = new ObservableCollection<Product>(Products);
        }

        public void RefreshOrders()
        {
            OnPropertyChanged(nameof(Orders));
            OnPropertyChanged(nameof(TotalRevenue));
        }

        private void FilterProducts()
        {
            var filtered = Products.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchTerm = SearchQuery.ToLower();
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Id.ToString().Contains(searchTerm) ||
                    p.Price.ToString("F2").Contains(searchTerm) ||
                    p.Category.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory))
            {
                filtered = filtered.Where(p => p.Category == SelectedCategory);
            }

            FilteredProducts = new ObservableCollection<Product>(filtered);
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

        private async void OnSelectProductForUpdate(Product product)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                $"Update: {product.Name}",
                "Cancel",
                null,
                "Edit Name",
                "Edit Price",
                "Edit Stock",
                "Edit Category",
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
                case "Edit Category":
                    await UpdateProductCategory(product);
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
                $"Product: {product.Name}\nCurrent price: ₱{product.Price:F2}\nEnter new price:",
                "Update",
                "Cancel",
                product.Price.ToString("F2"),
                keyboard: Keyboard.Numeric);

            if (decimal.TryParse(newPriceStr, out decimal newPrice) && newPrice >= 0)
            {
                product.Price = newPrice;
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Price updated to: ₱{newPrice:F2}", "OK");
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

        private async Task UpdateProductCategory(Product product)
        {
            string selectedCategory = await Application.Current.MainPage.DisplayActionSheet(
                $"Update Category for {product.Name}\nCurrent: {product.Category}",
                "Cancel",
                null,
                DataService.Categories.ToArray());

            if (selectedCategory != "Cancel" && !string.IsNullOrEmpty(selectedCategory))
            {
                product.Category = selectedCategory;
                await Application.Current.MainPage.DisplayAlert("Success",
                    $"Category updated to: {selectedCategory}", "OK");
            }
        }

        private async Task UpdateAllFields(Product product)
        {
            string newName = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 1/4",
                $"Current name: {product.Name}\nEnter new name:",
                "Next",
                "Cancel",
                product.Name);
            if (newName == null) return;

            string selectedCategory = await Application.Current.MainPage.DisplayActionSheet(
                "Update Product - Step 2/4\nSelect Category",
                "Cancel",
                null,
                DataService.Categories.ToArray());
            if (selectedCategory == "Cancel" || selectedCategory == null) return;

            string newPriceStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 3/4",
                $"Current price: ₱{product.Price:F2}\nEnter new price:",
                "Next",
                "Cancel",
                product.Price.ToString("F2"),
                keyboard: Keyboard.Numeric);
            if (newPriceStr == null) return;

            string newStockStr = await Application.Current.MainPage.DisplayPromptAsync(
                "Update Product - Step 4/4",
                $"Current stock: {product.StockQuantity}\nEnter new stock:",
                "Update",
                "Cancel",
                product.StockQuantity.ToString(),
                keyboard: Keyboard.Numeric);
            if (newStockStr == null) return;

            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;
            if (!string.IsNullOrWhiteSpace(selectedCategory))
                product.Category = selectedCategory;
            if (decimal.TryParse(newPriceStr, out decimal newPrice) && newPrice >= 0)
                product.Price = newPrice;
            if (int.TryParse(newStockStr, out int newStock) && newStock >= 0)
                product.StockQuantity = newStock;

            await Application.Current.MainPage.DisplayAlert("Success",
                "All product details updated successfully!", "OK");
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

            DataService.UpdateOrderStatus(order.OrderNumber, newStatus);
            OnPropertyChanged(nameof(Orders));

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
                StockQuantity = stock,
                Category = string.IsNullOrWhiteSpace(NewProductCategory) ? "N/A" : NewProductCategory
            };

            DataService.Products.Add(newProduct);
            FilterProducts();

            NewProductName = string.Empty;
            NewProductPrice = string.Empty;
            NewProductStock = string.Empty;
            NewProductCategory = string.Empty;
            OnPropertyChanged(nameof(NewProductName));
            OnPropertyChanged(nameof(NewProductPrice));
            OnPropertyChanged(nameof(NewProductStock));
            OnPropertyChanged(nameof(NewProductCategory));

            Application.Current.MainPage.DisplayAlert("Success",
                $"Product '{newProduct.Name}' added successfully!", "OK");
        }

        private void OnAddCategory()
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Enter category name", "OK");
                return;
            }

            if (DataService.Categories.Contains(NewCategoryName))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Category already exists", "OK");
                return;
            }

            DataService.Categories.Add(NewCategoryName);
            Categories = new ObservableCollection<string>(DataService.Categories);
            NewCategoryName = string.Empty;
            OnPropertyChanged(nameof(NewCategoryName));
            Application.Current.MainPage.DisplayAlert("Success", "Category added!", "OK");
        }

        private async void OnEditCategory(string category)
        {
            string newName = await Application.Current.MainPage.DisplayPromptAsync(
                "Edit Category", "Enter new name:", "Save", "Cancel", category);

            if (!string.IsNullOrWhiteSpace(newName) && newName != category)
            {
                var index = DataService.Categories.IndexOf(category);
                if (index >= 0)
                {
                    foreach (var product in DataService.Products.Where(p => p.Category == category))
                    {
                        product.Category = newName;
                    }
                    DataService.Categories[index] = newName;
                    Categories = new ObservableCollection<string>(DataService.Categories);
                    FilterProducts();
                }
            }
        }

        private async void OnDeleteCategory(string category)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Category",
                $"Delete '{category}'? Products will become 'N/A'.",
                "Delete", "Cancel");

            if (confirm)
            {
                DataService.Categories.Remove(category);
                foreach (var product in DataService.Products.Where(p => p.Category == category))
                {
                    product.Category = "N/A";
                }
                Categories = new ObservableCollection<string>(DataService.Categories);
                FilterProducts();
            }
        }

        private async Task OnLogout()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Logout", "Are you sure you want to logout?", "Yes", "No");

            if (confirm)
            {
                DataService.CurrentUser = null;
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}