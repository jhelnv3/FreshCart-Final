using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FreshCart.Models
{
    public class ProductDisplay : INotifyPropertyChanged
    {
        private int _selectedQuantity;
        private Product _product;

        public Product Product
        {
            get => _product;
            set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(StockQuantity));
                    OnPropertyChanged(nameof(Id));
                    OnPropertyChanged(nameof(IsInStock));
                }
            }
        }

        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set
            {
                if (_selectedQuantity != value)
                {
                    _selectedQuantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => Product?.Name;
        public decimal Price => Product?.Price ?? 0;
        public int StockQuantity => Product?.StockQuantity ?? 0;
        public int Id => Product?.Id ?? 0;
        public bool IsInStock => Product?.StockQuantity > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}