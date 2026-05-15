using FreshCart.ViewModels;

namespace FreshCart.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = new HistoryViewModel();
    }
}