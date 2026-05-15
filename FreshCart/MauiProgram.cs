using Microsoft.Extensions.Logging;

using FreshCart.ViewModels;
using FreshCart.Views;


namespace FreshCart;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<UserViewModel>();
        builder.Services.AddTransient<CartViewModel>();
        builder.Services.AddTransient<StatusViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<StaffViewModel>();

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<UserPage>();
        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<StatusPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<StaffPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}