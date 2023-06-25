using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaWeatherTracker.Models;
using DynamicData;
using Newtonsoft.Json;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherService
{
    private static HttpClient _httpClient = new();
    private static string ApiUrl = "http://localhost:5143/Main";

    public static string Status { get; set; } = "";
    
    public static async Task GetReports(ObservableCollection<WeatherReport> reports)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiUrl);
        
            if (!response.IsSuccessStatusCode) return;
        
            var json = response.Content.ReadAsStringAsync().Result;
            reports.Clear();
            reports.AddRange(JsonConvert.DeserializeObject<List<WeatherReport>>(json));

            Status = $"Last fetched at: {DateTimeFormatter.FormatToString(DateTime.Now)}";    
        }
        catch (Exception)
        {
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
        
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, newCityReport);
            response.EnsureSuccessStatusCode();

            Status = "New city successfully added!";
        }
        catch (Exception)
        {
            Status = "Server out of reach.";
        }
    }
}
