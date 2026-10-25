using InventoryApp.Services;
using InventoryApp.ViewModels;
using InventoryApp.Views;
using Microsoft.Extensions.Logging;

namespace InventoryApp
{
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
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<ProductDetailsViewModel>();
            builder.Services.AddTransient<ProductDetailsPage>();
            builder.Services.AddTransient<AddProductViewModel>();
            builder.Services.AddTransient<AddProductPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}