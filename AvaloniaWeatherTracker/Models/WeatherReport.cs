using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AvaloniaWeatherTracker.Models;

public class WeatherReport
{
    public string? Icon { get; set; }
    public Guid Id { get; set; }
    public string City { get; set; }
    public long DegreesCelsius { get; set; }
    public long DegreesFahrenheit { get; set; }
    public long WindSpeed { get; set; }
}