using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FreshCart.Models
{
    public class Order : INotifyPropertyChanged
    {
        private OrderStatus _status;

        public string OrderNumber { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ReceivedDate { get; set; }

        public OrderStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    if (value == OrderStatus.Received)
                    {
                        ReceivedDate = DateTime.Now;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum OrderStatus
    {
        Pending,
        Packing,
        ToDeliver,
        ToReceive,
        Delivered,
        Received
    }
}