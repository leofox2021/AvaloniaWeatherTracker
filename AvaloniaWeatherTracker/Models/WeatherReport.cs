using System;

namespace AvaloniaWeatherTracker.Models;

public class WeatherReport
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public long DegreesCelsius { get; set; }
    public long DegreesFahrenheit { get; set; }
    public long WindSpeed { get; set; }
}