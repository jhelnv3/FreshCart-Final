using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreshCart.Services;
using FreshCart.Views;
using System.Windows.Input;

namespace FreshCart.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        private string _username;
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
        }

        private async void OnLogin()
        {
            var user = DataService.ValidateLogin(Username, Password);
            if (user != null)
            {
                DataService.CurrentUser = user;
                ErrorMessage = string.Empty;

                if (user.Role == "User")
                {
                    await Shell.Current.GoToAsync("//UserPage");
                }
                else if (user.Role == "Staff")
                {
                    await Shell.Current.GoToAsync("//StaffPage");
                }
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }
    }
}
