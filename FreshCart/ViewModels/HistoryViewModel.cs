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
    public class HistoryViewModel : BindableObject
    {
        public ObservableCollection<Order> Orders => DataService.Orders;
        public ICommand LogoutCommand { get; }

        public HistoryViewModel()
        {
            LogoutCommand = new Command(async () => await OnLogout());
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
