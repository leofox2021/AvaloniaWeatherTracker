using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaWeatherTracker.Models;
using DynamicData;
using Newtonsoft.Json;
using WeatherForecastRestAPI.Model;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherService
{
    private static HttpClient _httpClient = new();
    private static string ApiUrlMain = "http://localhost:5143/Main";
    private static string ApiUrlExtended = "http://localhost:5143/Extended";

    public static string Status { get; set; } = "";
    
    public static async Task GetReports(ObservableCollection<WeatherReport> reports, 
        ObservableCollection<ExtendedWeatherReport> extendedReports)
    {
        try
        {
            List<WeatherReport> temporaryReportList = new List<WeatherReport>();
            var response = await _httpClient.GetAsync(ApiUrlMain);
            var extendedResponse = await _httpClient.GetAsync(ApiUrlExtended);
        
            if (!response.IsSuccessStatusCode) return;
        
            var json = response.Content.ReadAsStringAsync().Result;
            reports.Clear();
            temporaryReportList.AddRange(JsonConvert.DeserializeObject<List<WeatherReport>>(json));
            

            foreach (var report in temporaryReportList)
                report.Icon = WeatherImagePicker.PickImageSource(report);

            reports.AddRange(temporaryReportList);
            
            var jsonExtended = extendedResponse.Content.ReadAsStringAsync().Result;
            extendedReports.Clear();
            extendedReports.AddRange(JsonConvert.DeserializeObject<List<ExtendedWeatherReport>>(jsonExtended));
            
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
            var newCityReport = new WeatherReport
            {
                Id = new Guid(),
                City = city,
                DegreesCelsius = 0,
                DegreesFahrenheit = 0,
                WindSpeed = 0
            };
        
            var response = await _httpClient.PostAsJsonAsync(ApiUrlMain, newCityReport);
            response.EnsureSuccessStatusCode();

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
            response.EnsureSuccessStatusCode();
            
            Status = "New city successfully added!";
        }
        catch (Exception exception)
        {
            // Debug.WriteLine("Error adding city by rest API!\n");
            // Debug.WriteLine(exception.Message);
            Status = "Server out of reach.";
        }
    }
    
    public static async Task RemoveCity(ObservableCollection<WeatherReport> weatherReports, 
        ObservableCollection<ExtendedWeatherReport> extendedWeatherReports, int index)
    {
        try
        {
            await _httpClient.DeleteAsync(ApiUrlMain + $"?id={weatherReports[index].Id}");
            await _httpClient.DeleteAsync(ApiUrlExtended + $"?id={extendedWeatherReports[index].Id}");
            Status = "City successfully deleted!.";
        }
        catch (Exception exception)
        {
            // Debug.WriteLine("Error deleting city by rest API!\n");
            // Debug.WriteLine(exception.Message);
            Status = "Server out of reach.";
            return;
        }
        
        try
        {
            weatherReports.RemoveAt(index);
            extendedWeatherReports.RemoveAt(index);
        }
        catch (Exception exception)
        {
            // Debug.WriteLine("Error deleting cities from the collections!\n");
            // Debug.WriteLine(exception.Message);
        }
    }
}
