using System.Globalization;

namespace InventoryApp.Converters;

public class QuantityToColorConverter : IValueConverter
{
    public Color OutOfStockColor { get; set; } = Colors.Red;
    public Color NormalColor { get; set; } = Colors.Gray;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal quantity)
        {
            return quantity <= 0 ? OutOfStockColor : NormalColor;
        }

        return NormalColor;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}