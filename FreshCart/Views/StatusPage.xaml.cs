using FreshCart.ViewModels;

namespace FreshCart.Views;

public partial class StatusPage : ContentPage
{
    private readonly StatusViewModel _viewModel;

    public StatusPage()
    {
        InitializeComponent();
        _viewModel = new StatusViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the orders display every time the page appears
        _viewModel.RefreshOrders();
    }
}