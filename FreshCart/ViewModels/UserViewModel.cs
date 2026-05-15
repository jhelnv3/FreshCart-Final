using FreshCart.Models;
using FreshCart.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FreshCart.ViewModels
{
    public class UserViewModel : BindableObject
    {
        private int _cartItemCount;
        private string _searchQuery;
        private ObservableCollection<ProductDisplay> _filteredProducts;

        public ObservableCollection<ProductDisplay> ProductDisplays { get; set; } = new();

        public ObservableCollection<ProductDisplay> FilteredProducts
        {
            get => _filteredProducts ?? ProductDisplays;
            set { _filteredProducts = value; OnPropertyChanged(); }
        }

        public bool HasItemsInCart => CartItemCount > 0;

        public int CartItemCount
        {
            get => _cartItemCount;
            set
            {
                _cartItemCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasItemsInCart));
            }
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

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand GoToCartCommand { get; }
        public ICommand GoToStatusCommand { get; }
        public ICommand GoToHistoryCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand SearchCommand { get; }

        public UserViewModel()
        {
            LoadProducts();

            IncreaseQuantityCommand = new Command<ProductDisplay>(OnIncreaseQuantity);
            DecreaseQuantityCommand = new Command<ProductDisplay>(OnDecreaseQuantity);
            AddToCartCommand = new Command<ProductDisplay>(OnAddToCart);
            GoToCartCommand = new Command(async () => await Shell.Current.GoToAsync("CartPage"));
            GoToStatusCommand = new Command(async () => await Shell.Current.GoToAsync("StatusPage"));
            GoToHistoryCommand = new Command(async () => await Shell.Current.GoToAsync("HistoryPage"));
            LogoutCommand = new Command(async () => await OnLogout());
            SearchCommand = new Command(FilterProducts);

            UpdateCartCount();
        }

        private void LoadProducts()
        {
            ProductDisplays.Clear();
            foreach (var product in DataService.Products)
            {
                ProductDisplays.Add(new ProductDisplay { Product = product });
            }
            FilteredProducts = new ObservableCollection<ProductDisplay>(ProductDisplays);
        }

        public void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredProducts = new ObservableCollection<ProductDisplay>(ProductDisplays);
            }
            else
            {
                var searchTerm = SearchQuery.ToLower();
                var filtered = ProductDisplays.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Price.ToString("F2").Contains(searchTerm)
                ).ToList();
                FilteredProducts = new ObservableCollection<ProductDisplay>(filtered);
            }
        }

        public void RefreshProductStock()
        {
            foreach (var productDisplay in ProductDisplays)
            {
                var product = DataService.Products.FirstOrDefault(p => p.Id == productDisplay.Id);
                if (product != null)
                {
                    productDisplay.Product = product;
                    productDisplay.SelectedQuantity = 0;
                }
            }

            foreach (var product in DataService.Products)
            {
                if (!ProductDisplays.Any(pd => pd.Id == product.Id))
                {
                    ProductDisplays.Add(new ProductDisplay { Product = product });
                }
            }

            FilterProducts();
            OnPropertyChanged(nameof(ProductDisplays));
        }

        public void UpdateCartCount()
        {
            CartItemCount = DataService.Cart.Sum(c => c.Quantity);
        }

        private void OnIncreaseQuantity(ProductDisplay productDisplay)
        {
            if (productDisplay.SelectedQuantity < productDisplay.StockQuantity)
            {
                productDisplay.SelectedQuantity++;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Stock Limit",
                    $"Only {productDisplay.StockQuantity} items available in stock", "OK");
            }
        }

        private void OnDecreaseQuantity(ProductDisplay productDisplay)
        {
            if (productDisplay.SelectedQuantity > 0)
            {
                productDisplay.SelectedQuantity--;
            }
        }

        private async void OnAddToCart(ProductDisplay productDisplay)
        {
            var latestProduct = DataService.Products.FirstOrDefault(p => p.Id == productDisplay.Id);
            if (latestProduct == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Product not found", "OK");
                return;
            }

            if (productDisplay.SelectedQuantity <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Please select a quantity greater than 0", "OK");
                return;
            }

            if (productDisplay.SelectedQuantity > latestProduct.StockQuantity)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Only {latestProduct.StockQuantity} items available in stock", "OK");
                return;
            }

            var existingItem = DataService.Cart.FirstOrDefault(c => c.Product.Id == productDisplay.Id);
            int currentCartQuantity = existingItem?.Quantity ?? 0;

            if (currentCartQuantity + productDisplay.SelectedQuantity > latestProduct.StockQuantity + currentCartQuantity)
            {
                await Application.Current.MainPage.DisplayAlert("Stock Limit",
                    $"Cannot add more. You already have {currentCartQuantity} in cart. Stock available: {latestProduct.StockQuantity}", "OK");
                return;
            }

            if (existingItem != null)
            {
                existingItem.Quantity += productDisplay.SelectedQuantity;
            }
            else
            {
                DataService.Cart.Add(new CartItem
                {
                    Product = latestProduct,
                    Quantity = productDisplay.SelectedQuantity
                });
            }

            latestProduct.StockQuantity -= productDisplay.SelectedQuantity;
            productDisplay.Product = latestProduct;

            int addedQuantity = productDisplay.SelectedQuantity;
            productDisplay.SelectedQuantity = 0;

            UpdateCartCount();

            // Refresh the filtered list to update stock display
            FilterProducts();

            await Application.Current.MainPage.DisplayAlert("Success",
                $"{addedQuantity}x {productDisplay.Name} added to cart", "OK");
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