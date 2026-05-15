using FreshCart.ViewModels;

namespace FreshCart.Views;

public partial class CartPage : ContentPage
{
    private readonly CartViewModel _viewModel;

    public CartPage()
    {
        InitializeComponent();
        _viewModel = new CartViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshTotal();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // When leaving cart page, notify UserPage to refresh
        MessagingCenter.Send<CartPage>(this, "CartUpdated");
    }
}