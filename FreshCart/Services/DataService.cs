using FreshCart.Models;
using System.Collections.ObjectModel;

namespace FreshCart.Services
{
    public static class DataService
    {
        public static ObservableCollection<Product> Products { get; set; } = new();
        public static ObservableCollection<CartItem> Cart { get; set; } = new();
        public static ObservableCollection<Order> Orders { get; set; } = new();
        public static ObservableCollection<string> Categories { get; set; } = new();
        public static User CurrentUser { get; set; }
        public static int OrderCounter = 1000;

        static DataService()
        {
            InitializeCategories();
            InitializeProducts();
        }

        private static void InitializeCategories()
        {
            Categories.Add("Fruits");
            Categories.Add("Vegetables");
            Categories.Add("Dairy & Eggs");
            Categories.Add("Meat & Seafood");
            Categories.Add("Pantry Essentials");
            Categories.Add("Beverages");
            Categories.Add("Bread & Bakery");
            Categories.Add("Snacks");
            Categories.Add("Canned Goods");
        }

        private static void InitializeProducts()
        {
            Products.Add(new Product { Id = 1, Name = "Fresh Apples (per piece)", Price = 25.00m, StockQuantity = 50, Category = "Fruits" });
            Products.Add(new Product { Id = 2, Name = "Lakatan Bananas (per kilo)", Price = 80.00m, StockQuantity = 40, Category = "Fruits" });
            Products.Add(new Product { Id = 3, Name = "Calamansi (per kilo)", Price = 120.00m, StockQuantity = 30, Category = "Fruits" });
            Products.Add(new Product { Id = 4, Name = "Green Mangoes (per kilo)", Price = 150.00m, StockQuantity = 25, Category = "Fruits" });
            Products.Add(new Product { Id = 5, Name = "Papaya (per kilo)", Price = 65.00m, StockQuantity = 35, Category = "Fruits" });

            Products.Add(new Product { Id = 6, Name = "Kangkong Bundle", Price = 20.00m, StockQuantity = 60, Category = "Vegetables" });
            Products.Add(new Product { Id = 7, Name = "Pechay Bundle", Price = 25.00m, StockQuantity = 55, Category = "Vegetables" });
            Products.Add(new Product { Id = 8, Name = "Siling Labuyo (per pack)", Price = 15.00m, StockQuantity = 70, Category = "Vegetables" });
            Products.Add(new Product { Id = 9, Name = "Red Onion (per kilo)", Price = 180.00m, StockQuantity = 30, Category = "Vegetables" });
            Products.Add(new Product { Id = 10, Name = "Garlic (per kilo)", Price = 140.00m, StockQuantity = 40, Category = "Vegetables" });
            Products.Add(new Product { Id = 11, Name = "Ginger (per kilo)", Price = 100.00m, StockQuantity = 35, Category = "Vegetables" });
            Products.Add(new Product { Id = 12, Name = "Tomatoes (per kilo)", Price = 90.00m, StockQuantity = 45, Category = "Vegetables" });

            Products.Add(new Product { Id = 13, Name = "Fresh Eggs (1 dozen)", Price = 110.00m, StockQuantity = 40, Category = "Dairy & Eggs" });
            Products.Add(new Product { Id = 14, Name = "Fresh Milk 1L", Price = 95.00m, StockQuantity = 35, Category = "Dairy & Eggs" });

            Products.Add(new Product { Id = 15, Name = "Chicken Whole (per kilo)", Price = 190.00m, StockQuantity = 25, Category = "Meat & Seafood" });
            Products.Add(new Product { Id = 16, Name = "Pork Belly (per kilo)", Price = 320.00m, StockQuantity = 20, Category = "Meat & Seafood" });
            Products.Add(new Product { Id = 17, Name = "Bangus (per piece)", Price = 150.00m, StockQuantity = 30, Category = "Meat & Seafood" });
            Products.Add(new Product { Id = 18, Name = "Tilapia (per piece)", Price = 100.00m, StockQuantity = 35, Category = "Meat & Seafood" });
            Products.Add(new Product { Id = 19, Name = "Shrimp Medium (per kilo)", Price = 450.00m, StockQuantity = 15, Category = "Meat & Seafood" });

            Products.Add(new Product { Id = 20, Name = "Rice Special (per kilo)", Price = 58.00m, StockQuantity = 100, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 21, Name = "Cooking Oil 1L", Price = 165.00m, StockQuantity = 40, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 22, Name = "Soy Sauce 1L", Price = 55.00m, StockQuantity = 50, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 23, Name = "Vinegar 1L", Price = 45.00m, StockQuantity = 50, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 24, Name = "Fish Sauce 750ml", Price = 65.00m, StockQuantity = 45, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 25, Name = "Sugar White (per kilo)", Price = 85.00m, StockQuantity = 50, Category = "Pantry Essentials" });
            Products.Add(new Product { Id = 26, Name = "Coffee 3-in-1 (10 sachets)", Price = 75.00m, StockQuantity = 40, Category = "Pantry Essentials" });

            Products.Add(new Product { Id = 27, Name = "Coca-Cola 1.5L", Price = 75.00m, StockQuantity = 45, Category = "Beverages" });
            Products.Add(new Product { Id = 28, Name = "Mineral Water 1L", Price = 25.00m, StockQuantity = 100, Category = "Beverages" });

            Products.Add(new Product { Id = 29, Name = "Tasty Bread Loaf", Price = 65.00m, StockQuantity = 30, Category = "Bread & Bakery" });
            Products.Add(new Product { Id = 30, Name = "Pandesal (10 pieces)", Price = 50.00m, StockQuantity = 40, Category = "Bread & Bakery" });

            Products.Add(new Product { Id = 31, Name = "Chippy BBQ", Price = 18.00m, StockQuantity = 80, Category = "Snacks" });
            Products.Add(new Product { Id = 32, Name = "Nova Cheese", Price = 18.00m, StockQuantity = 80, Category = "Snacks" });
            Products.Add(new Product { Id = 33, Name = "Piattos Cheese", Price = 18.00m, StockQuantity = 75, Category = "Snacks" });

            Products.Add(new Product { Id = 34, Name = "Corned Beef 150g", Price = 38.00m, StockQuantity = 60, Category = "Canned Goods" });
            Products.Add(new Product { Id = 35, Name = "Meat Loaf 150g", Price = 32.00m, StockQuantity = 55, Category = "Canned Goods" });
            Products.Add(new Product { Id = 36, Name = "Sardines 155g", Price = 25.00m, StockQuantity = 70, Category = "Canned Goods" });
            Products.Add(new Product { Id = 37, Name = "Tuna Flakes 155g", Price = 35.00m, StockQuantity = 65, Category = "Canned Goods" });
        }

        public static User ValidateLogin(string username, string password)
        {
            if (username == "user" && password == "user123")
            {
                return new User { Username = "user", Password = "user123", Role = "User", FullName = "Juan Dela Cruz" };
            }
            else if (username == "staff" && password == "staff123")
            {
                return new User { Username = "staff", Password = "staff123", Role = "Staff", FullName = "Maria Santos" };
            }
            return null;
        }

        public static string PlaceOrder(string paymentMethod)
        {
            var orderNumber = $"ORD-{++OrderCounter}";
            var order = new Order
            {
                OrderNumber = orderNumber,
                Items = new List<CartItem>(Cart),
                TotalAmount = Cart.Sum(i => i.TotalPrice),
                PaymentMethod = paymentMethod,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now
            };
            Orders.Add(order);
            Cart.Clear();
            return orderNumber;
        }

        public static void UpdateOrderStatus(string orderNumber, OrderStatus status)
        {
            var order = Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (order != null)
            {
                order.Status = status;
            }
        }
    }
}