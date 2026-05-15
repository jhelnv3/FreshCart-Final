using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Models
{
    public class Product : BindableObject
    {
        private int _stockQuantity;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "dotnet_bot.png";

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
            }
        }

        public bool IsInStock => StockQuantity > 0;
    }
}