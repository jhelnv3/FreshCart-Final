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
        RefreshData();
    }

    private void RefreshData()
    {
        // Refresh the viewmodel data
        _viewModel.RefreshOrders();
    }

    private void HideAllContent()
    {
        OrdersContent.IsVisible = false;
        AddProductContent.IsVisible = false;
        UpdateContent.IsVisible = false;
        CategoryContent.IsVisible = false;
        SummaryContent.IsVisible = false;
    }

    private void ResetTabColors()
    {
        OrdersTab.BackgroundColor = Color.FromArgb("#757575");
        AddProductTab.BackgroundColor = Color.FromArgb("#757575");
        UpdateTab.BackgroundColor = Color.FromArgb("#757575");
        CategoryTab.BackgroundColor = Color.FromArgb("#757575");
        SummaryTab.BackgroundColor = Color.FromArgb("#757575");
    }

    private void OnOrdersTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        OrdersContent.IsVisible = true;
        ResetTabColors();
        OrdersTab.BackgroundColor = Color.FromArgb("#2196F3");
        _viewModel.RefreshOrders();
    }

    private void OnAddProductTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        AddProductContent.IsVisible = true;
        ResetTabColors();
        AddProductTab.BackgroundColor = Color.FromArgb("#2196F3");
    }

    private void OnProductsTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        ResetTabColors();
    }

    private void OnUpdateTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        UpdateContent.IsVisible = true;
        ResetTabColors();
        UpdateTab.BackgroundColor = Color.FromArgb("#2196F3");
    }

    private void OnCategoryTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        CategoryContent.IsVisible = true;
        ResetTabColors();
        CategoryTab.BackgroundColor = Color.FromArgb("#2196F3");
    }

    private void OnSummaryTabClicked(object sender, EventArgs e)
    {
        HideAllContent();
        SummaryContent.IsVisible = true;
        ResetTabColors();
        SummaryTab.BackgroundColor = Color.FromArgb("#2196F3");
        _viewModel.RefreshOrders();
    }
}