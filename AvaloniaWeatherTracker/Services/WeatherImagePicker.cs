using System.IO;
using AvaloniaWeatherTracker.Models;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherImagePicker
{
    public static string PickImageSource(WeatherReport weatherReport) => weatherReport.DegreesCelsius switch
    {
        > 25 => Directory.GetCurrentDirectory() + "/Resources/sunny.png",
        < 25 and > 10 => Directory.GetCurrentDirectory() + "/Resources/cloudy.png",
        < 10 and > 0 => Directory.GetCurrentDirectory() + "/Resources/super_cloudy.png",
        < 0 => Directory.GetCurrentDirectory() + "/Resources/snow.png",
        _ => Directory.GetCurrentDirectory() + "/Resources/sunny.png"
    };
}