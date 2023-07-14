using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using AvaloniaWeatherTracker.Enums;
using AvaloniaWeatherTracker.Models;
using DynamicData;
using Newtonsoft.Json;

namespace AvaloniaWeatherTracker.Services;

public static class WeatherService
{
    private static HttpClient _httpClient = new();
    private const string ApiUrlExtended = "http://localhost:5143/Extended";
    private const string ApiUrlWeekly = "http://localhost:5143/Weekly?city=";
    private const int DaysInWeeklyReport = 7;

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
            {
                report.Icon = WeatherImagePicker.PickImageSource(report.DegreesCelsius);
                report.Weekly = GetWeeklyReports(report.City).Result;
            }

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
    
    private static async Task<List<WeekdayWeatherRecord>> GetWeeklyReports(string city)
    {
        try
        {
            var records = new List<WeekdayWeatherRecord>();
            var url = $"{ApiUrlWeekly}{city}";
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode) return null;
            
            var json = response.Content.ReadAsStringAsync().Result;
            var degrees = JsonConvert.DeserializeObject<int[]>(json);

            for (int i = 0; i < DaysInWeeklyReport; i++)
            {
                records.Add(new WeekdayWeatherRecord()
                {
                    Temperature = degrees[i],
                    Day = $"{(Month)DateTime.Now.Month}. {DateTime.Now.Day + i}",
                    Icon = WeatherImagePicker.PickImageSource(degrees[i])
                });
            }

            return records;
        }
        catch (Exception exception)
        {
            Debug.WriteLine("Error getting weekly reports from rest API!\n");
            Debug.WriteLine(exception.Message);
            return null;
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
