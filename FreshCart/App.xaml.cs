namespace FreshCart;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Force Light Mode
        Application.Current.UserAppTheme = AppTheme.Light;

        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();
        // Ensure light mode is maintained
        Application.Current.UserAppTheme = AppTheme.Light;
    }
}