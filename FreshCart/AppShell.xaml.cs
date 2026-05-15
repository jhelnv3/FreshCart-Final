namespace FreshCart;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("CartPage", typeof(Views.CartPage));
        Routing.RegisterRoute("StatusPage", typeof(Views.StatusPage));
        Routing.RegisterRoute("HistoryPage", typeof(Views.HistoryPage));
    }
}