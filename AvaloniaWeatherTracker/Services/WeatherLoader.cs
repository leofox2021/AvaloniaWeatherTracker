using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaWeatherTracker.Models;
using DynamicData;
using Newtonsoft.Json;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherLoader
{
    private static HttpClient _httpClient = new();
    private static string ApiUrl = "http://localhost:5143/Main";

    public static string LastFetchDate { get; set; } = "";
    
    public static async Task GetReports(ObservableCollection<WeatherReport> reports)
    {
        var response = await _httpClient.GetAsync(ApiUrl);
        
        if (!response.IsSuccessStatusCode) return;
        
        var json = response.Content.ReadAsStringAsync().Result;
        reports.Clear();
        reports.AddRange(JsonConvert.DeserializeObject<List<WeatherReport>>(json));

        LastFetchDate = $"Last fetched at: {DateTimeFormatter.FormatToString(DateTime.Now)}";
    }
}
