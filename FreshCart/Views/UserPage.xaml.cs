using FreshCart.ViewModels;

namespace FreshCart.Views;

public partial class UserPage : ContentPage
{
    private readonly UserViewModel _viewModel;

    public UserPage()
    {
        InitializeComponent();
        _viewModel = new UserViewModel();
        BindingContext = _viewModel;

        MessagingCenter.Subscribe<CartPage>(this, "CartUpdated", (sender) =>
        {
            RefreshPageData();
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshPageData();
    }

    private void RefreshPageData()
    {
        _viewModel.UpdateCartCount();
        _viewModel.RefreshProductStock();
        OnPropertyChanged(nameof(BindingContext));
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<CartPage>(this, "CartUpdated");
    }
}