using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp.ViewModels;

public partial class AddProductViewModel : ObservableObject
{
    private readonly IProductService _productService;

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

    public AddProductViewModel(IProductService productService)
    {
        _productService = productService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "A terméknév megadása kötelező!";
            return;
        }

        if (string.IsNullOrWhiteSpace(Barcode))
        {
            ErrorMessage = "A vonalkód megadása kötelező!";
            return;
        }

        if (!decimal.TryParse(PriceText, out var price) || price < 0)
        {
            ErrorMessage = "Érvénytelen ár! Pozitív számot adj meg.";
            return;
        }

        if (!decimal.TryParse(QuantityText, out var quantity) || quantity < 0)
        {
            ErrorMessage = "Érvénytelen mennyiség! Pozitív számot adj meg.";
            return;
        }

        var newProduct = new Product
        {
            Name = Name.Trim(),
            Barcode = Barcode.Trim(),
            Price = price,
            Quantity = quantity
        };

        await _productService.SaveProductAsync(newProduct);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}