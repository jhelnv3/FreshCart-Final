using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FreshCart.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _stockQuantity;
        private string _category;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "dotnet_bot.png";
        public string Category
        {
            get => string.IsNullOrWhiteSpace(_category) ? "N/A" : _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsInStock));
            }
        }

        public bool IsInStock => StockQuantity > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}