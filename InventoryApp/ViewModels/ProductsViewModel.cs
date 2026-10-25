using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Services;
using InventoryApp.Views;
using System.Collections.ObjectModel;

namespace InventoryApp.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private CancellationTokenSource? _searchCts;

    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _searchCts?.Cancel();
                _searchCts = new CancellationTokenSource();
                var token = _searchCts.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(300, token);
                        await LoadProductsAsync();
                    }
                    catch (TaskCanceledException)
                    {
                    }
                });
            }
        }
    }

    [ObservableProperty]
    private bool _isBusy;

    public ProductsViewModel(IProductService productService)
    {
        _productService = productService;
    }

    [RelayCommand]
    public async Task LoadProductsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var list = await _productService.GetProductsAsync(SearchText);
            Products = new ObservableCollection<Product>(list);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddProductAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddProductPage));
    }

    [RelayCommand]
    private async Task ProductSelectedAsync(Product? product)
    {
        if (product is null) return;

        await Shell.Current.GoToAsync($"{nameof(ProductDetailsPage)}?id={product.Id}");
    }
}