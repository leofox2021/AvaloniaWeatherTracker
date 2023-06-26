using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace AvaloniaWeatherTracker.Models;

public class WeatherReport
{
    public Image Icon { get; set; }
    public Guid Id { get; set; }
    public string City { get; set; }
    public long DegreesCelsius { get; set; }
    public long DegreesFahrenheit { get; set; }
    public long WindSpeed { get; set; }
    
    public void GenerateImage()
    {
        var rootResourceFolder = @$"{Directory.GetCurrentDirectory()}\Resources\snow.png";
        
        Icon = new Image();

        if (DegreesCelsius > 25)
        {
            using (var fileStream = new FileStream(rootResourceFolder, FileMode.OpenOrCreate))
            {
                Icon.Source = new Bitmap(fileStream);
            }
        }
        else if (DegreesCelsius < 25 && DegreesCelsius > 10)
        {
            using (var fileStream = new FileStream(rootResourceFolder, FileMode.OpenOrCreate))
            {
                Icon.Source = new Bitmap(fileStream);
            }
        }
        else if (DegreesCelsius < 10 && DegreesCelsius > 0)
        {
            using (var fileStream = new FileStream(rootResourceFolder, FileMode.OpenOrCreate))
            {
                Icon.Source = new Bitmap(fileStream);
            }
        }
        else
        {
            using (var fileStream = new FileStream(rootResourceFolder, FileMode.OpenOrCreate))
            {
                Icon.Source = new Bitmap(fileStream);
            }
        }
    }
}