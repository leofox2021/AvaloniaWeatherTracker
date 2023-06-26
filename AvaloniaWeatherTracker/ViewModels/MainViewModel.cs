using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    private string _status;

    [ObservableProperty]
    private string _textBotText;

    [ObservableProperty]
    private int _selectedIndex;
    
    public MainViewModel()
    {
        WeatherReports = new ObservableCollection<WeatherReport>();
        
        LoadReports = new RelayCommand(DoLoadWeatherReports, CanDoLoadWeatherReports);
        AddCity = new RelayCommand(DoAddCity, CanDoAddCity);
        RemoveCity = new RelayCommand(DoRemoveCity, CanDoRemoveCity);
        
        _status = "Last fetched at: never fetched!";
        _textBotText = "";
    }

    public ICommand LoadReports { get; set; }
    public ICommand AddCity { get; set; }
    public ICommand RemoveCity { get; set; }
    
    private bool CanDoLoadWeatherReports(object obj) => true;

    private async void DoLoadWeatherReports(object obj)
    {
        await WeatherService.GetReports(WeatherReports);
        Status = WeatherService.Status;
    }
    
    private bool CanDoAddCity(object obj) => true;
    
    private async void DoAddCity(object obj)
    {
        await WeatherService.AddNewCity(TextBotText);
        Status = WeatherService.Status;
    }
    
    private bool CanDoRemoveCity(object obj) => true;
    
    private async void DoRemoveCity(object obj)
    {
        await WeatherService.RemoveCity(WeatherReports, SelectedIndex);
        Status = WeatherService.Status;
    }
}