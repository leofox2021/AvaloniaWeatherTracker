using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace AvaloniaWeatherTracker.Services;

public class BitmapConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        new Bitmap((string) value);
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => new();
}