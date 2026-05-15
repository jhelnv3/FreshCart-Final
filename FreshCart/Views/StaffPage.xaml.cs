using FreshCart.ViewModels;

namespace FreshCart.Views;

public partial class StaffPage : ContentPage
{
    private readonly StaffViewModel _viewModel;

    public StaffPage()
    {
        InitializeComponent();
        _viewModel = new StaffViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh data when page appears
        RefreshOrdersDisplay();
    }

    private void RefreshOrdersDisplay()
    {
        // Force UI refresh for orders
        if (_viewModel != null)
        {
            var ordersProperty = _viewModel.GetType().GetProperty("Orders");
            if (ordersProperty != null)
            {
                _viewModel.GetType().GetMethod("OnPropertyChanged")?.Invoke(_viewModel, new[] { "Orders" });
            }
        }
    }

    private void OnOrdersTabClicked(object sender, EventArgs e)
    {
        OrdersContent.IsVisible = true;
        AddProductContent.IsVisible = false;
        ProductsContent.IsVisible = false;
        UpdateContent.IsVisible = false;

        OrdersTab.BackgroundColor = Color.FromArgb("#2196F3");
        AddProductTab.BackgroundColor = Color.FromArgb("#757575");
        ProductsTab.BackgroundColor = Color.FromArgb("#757575");
        UpdateTab.BackgroundColor = Color.FromArgb("#757575");

        // Refresh orders when switching to Orders tab
        RefreshOrdersDisplay();
    }

    private void OnAddProductTabClicked(object sender, EventArgs e)
    {
        OrdersContent.IsVisible = false;
        AddProductContent.IsVisible = true;
        ProductsContent.IsVisible = false;
        UpdateContent.IsVisible = false;

        OrdersTab.BackgroundColor = Color.FromArgb("#757575");
        AddProductTab.BackgroundColor = Color.FromArgb("#2196F3");
        ProductsTab.BackgroundColor = Color.FromArgb("#757575");
        UpdateTab.BackgroundColor = Color.FromArgb("#757575");
    }

    private void OnProductsTabClicked(object sender, EventArgs e)
    {
        OrdersContent.IsVisible = false;
        AddProductContent.IsVisible = false;
        ProductsContent.IsVisible = true;
        UpdateContent.IsVisible = false;

        OrdersTab.BackgroundColor = Color.FromArgb("#757575");
        AddProductTab.BackgroundColor = Color.FromArgb("#757575");
        ProductsTab.BackgroundColor = Color.FromArgb("#2196F3");
        UpdateTab.BackgroundColor = Color.FromArgb("#757575");
    }

    private void OnUpdateTabClicked(object sender, EventArgs e)
    {
        OrdersContent.IsVisible = false;
        AddProductContent.IsVisible = false;
        ProductsContent.IsVisible = false;
        UpdateContent.IsVisible = true;

        OrdersTab.BackgroundColor = Color.FromArgb("#757575");
        AddProductTab.BackgroundColor = Color.FromArgb("#757575");
        ProductsTab.BackgroundColor = Color.FromArgb("#757575");
        UpdateTab.BackgroundColor = Color.FromArgb("#2196F3");
    }
}