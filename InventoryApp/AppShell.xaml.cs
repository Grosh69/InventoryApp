using InventoryApp.Views;

namespace InventoryApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            InitializeComponent();
        }
    }
}