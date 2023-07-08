using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaWeatherTracker.Models;
using DynamicData;
using Newtonsoft.Json;
using AvaloniaWeatherTracker.Models;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherService
{
    private static HttpClient _httpClient = new();
    private static string ApiUrlExtended = "http://localhost:5143/Extended";

    public static string Status { get; set; } = "";
    
    public static async Task GetReports(ObservableCollection<ExtendedWeatherReport> extendedReports)
    {
        try
        {
            var temporaryReportList = new List<ExtendedWeatherReport>();
            var extendedResponse = await _httpClient.GetAsync(ApiUrlExtended);
        
            if (!extendedResponse.IsSuccessStatusCode) return;
        
            var json = extendedResponse.Content.ReadAsStringAsync().Result;
            extendedReports.Clear();
            temporaryReportList.AddRange(JsonConvert.DeserializeObject<List<ExtendedWeatherReport>>(json));
            

            foreach (var report in temporaryReportList)
                report.Icon = WeatherImagePicker.PickImageSource(report);

            extendedReports.AddRange(temporaryReportList);

            Status = $"Last fetched at: {DateTimeFormatter.FormatToString(DateTime.Now)}";    
        }
        catch (Exception exception)
        {
            Debug.WriteLine("Error getting reports from rest API!\n");
            Debug.WriteLine(exception.Message);
            Status = "Server out of reach.";
        }
    }
    
    public static async Task AddNewCity(string city)
    {
        try
        {
            var newCityExtendedReport = new ExtendedWeatherReport
            {
                Id = new Guid(),
                City = city,
                DegreesCelsius = 0,
                DegreesFahrenheit = 0,
                WindSpeed = 0,
                Country = "none",
                CountryCode = "none",
                Elevation = 0,
                Population = 0, 
                Timezone = "none",
                RainSum = 0,
                Precipitation = 0,
                Showers = 0,
                Snowfall = 0,
                Sunset = "none",
                Sunrise = "none",
                WindDirection = 0
            };
            
            await _httpClient.PostAsJsonAsync(ApiUrlExtended, newCityExtendedReport);
            Status = "New city successfully added!";
        }
        catch (Exception exception)
        {
            Debug.WriteLine("Error adding city by rest API!\n");
            Debug.WriteLine(exception.Message);
            Status = "Server out of reach.";
        }
    }
    
    public static async Task RemoveCity(ObservableCollection<ExtendedWeatherReport> extendedWeatherReports, int index)
    {
        try
        {
            await _httpClient.DeleteAsync(ApiUrlExtended + $"?id={extendedWeatherReports[index].Id}");
            Status = "City successfully deleted!.";
        }
        catch (Exception exception)
        {
            Debug.WriteLine("Error deleting city by rest API!\n");
            Debug.WriteLine(exception.Message);
            Status = "Server out of reach.";
            return;
        }
        
        try
        {
            extendedWeatherReports.RemoveAt(index);
        }
        catch (Exception exception)
        {
            Debug.WriteLine("Error deleting cities from the collections!\n");
            Debug.WriteLine(exception.Message);
        }
    }
}
