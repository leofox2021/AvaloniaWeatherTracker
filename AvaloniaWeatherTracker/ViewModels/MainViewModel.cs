using System.Collections.ObjectModel;
using System.Windows.Input;
using AvaloniaWeatherTracker.Commands;
using AvaloniaWeatherTracker.Models;
using AvaloniaWeatherTracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaWeatherTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<WeatherReport> _weatherReports;

    [ObservableProperty]
    private string _lastFetchDate;
    
    public MainViewModel()
    {
        WeatherReports = new ObservableCollection<WeatherReport>();
        LoadReports = new RelayCommand(DoLoadWeatherReports, CanDoLoadWeatherReports);
        _lastFetchDate = "Last fetched at: never fetched!";
    }

    public string ButtonText => "Load weather";

    public ICommand LoadReports { get; set; }
    
    private bool CanDoLoadWeatherReports(object obj) => true;

    private async void DoLoadWeatherReports(object obj)
    {
        await WeatherLoader.GetReports(WeatherReports);
        LastFetchDate = WeatherLoader.LastFetchDate;
    }
}
