using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels;

[QueryProperty("ProductId", "id")]
public partial class ProductDetailsViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private Product? _currentProduct;

    // KÉZI PROPERTY (itt ne legyen [ObservableProperty] és ne legyen másik _productId mező!)
    private int _productId;

    public int ProductId
    {
        get => _productId;
        set
        {
            if (SetProperty(ref _productId, value))
            {
                _ = LoadProductDetailsAsync(value);
            }
        }
    }

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _barcode = string.Empty;

    [ObservableProperty]
    private string _priceText = string.Empty;

    [ObservableProperty]
    private string _quantityText = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public ProductDetailsViewModel(IProductService productService)
    {
        _productService = productService;
    }

    private async Task LoadProductDetailsAsync(int id)
    {
        IsBusy = true;
        try
        {
            _currentProduct = await _productService.GetProductByIdAsync(id);
            if (_currentProduct is not null)
            {
                Name = _currentProduct.Name;
                Barcode = _currentProduct.Barcode;
                PriceText = _currentProduct.Price.ToString();
                QuantityText = _currentProduct.Quantity.ToString();
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (_currentProduct is null)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "A név nem lehet üres!";
            return;
        }

        if (string.IsNullOrWhiteSpace(Barcode))
        {
            ErrorMessage = "A vonalkód nem lehet üres!";
            return;
        }

        if (!decimal.TryParse(PriceText, out var price) || price < 0)
        {
            ErrorMessage = "Érvénytelen ár!";
            return;
        }

        if (!decimal.TryParse(QuantityText, out var quantity) || quantity < 0)
        {
            ErrorMessage = "Érvénytelen mennyiség!";
            return;
        }

        _currentProduct.Name = Name.Trim();
        _currentProduct.Barcode = Barcode.Trim();
        _currentProduct.Price = price;
        _currentProduct.Quantity = quantity;

        await _productService.SaveProductAsync(_currentProduct);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}