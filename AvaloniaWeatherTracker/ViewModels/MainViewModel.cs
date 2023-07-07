using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia;
using AvaloniaWeatherTracker.Commands;
using AvaloniaWeatherTracker.Models;
using AvaloniaWeatherTracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using WeatherForecastRestAPI.Model;

namespace AvaloniaWeatherTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(SelectedExtendedWeatherReport))]    
    private ObservableCollection<WeatherReport> _weatherReports;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedExtendedWeatherReport))]    
    private ObservableCollection<ExtendedWeatherReport> _extendedWeatherReports;

    [ObservableProperty] 
    private Thickness _flyoutMargin; 
    
    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private string _textBotText;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedExtendedWeatherReport))]    
    private int _selectedIndex;

    [ObservableProperty]
    private double _shadowRectangleOpacity;

    [ObservableProperty]
    private bool _isShadowRectangleEnabled;

    private const double FlyoutClosedMargin = 800;
    private const double FlyoutOpenMargin = 100;
    private const double FlyoutClosedOpacity = 0;
    private const double FlyoutOpenOpacity = 0.6;
    
    public MainViewModel()
    {
        WeatherReports = new ObservableCollection<WeatherReport>();
        ExtendedWeatherReports = new ObservableCollection<ExtendedWeatherReport>();
        
        LoadReports = new RelayCommand(DoLoadWeatherReports, CanDoLoadWeatherReports);
        AddCity = new RelayCommand(DoAddCity, CanDoAddCity);
        RemoveCity = new RelayCommand(DoRemoveCity, CanDoRemoveCity);
        ShowWeatherFlyout = new RelayCommand(DoShowWeatherFlyout, CanShowWeatherFlyout);
        HideWeatherFlyout = new RelayCommand(DoHideWeatherFlyout, CanHideWeatherFlyout);
        
        _status = "Last fetched at: never fetched!";
        _textBotText = "";
        _flyoutMargin = new Thickness(FlyoutClosedMargin, 0, 0, 0);
        _shadowRectangleOpacity = FlyoutClosedOpacity;
        _isShadowRectangleEnabled = false;
    }

    public ExtendedWeatherReport? SelectedExtendedWeatherReport
    {
        get
        {
            try
            {
                return ExtendedWeatherReports[SelectedIndex];
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    } 
    
    public ICommand LoadReports { get; set; }
    public ICommand AddCity { get; set; }
    public ICommand RemoveCity { get; set; }
    public ICommand ShowWeatherFlyout { get; set; }  
    public ICommand HideWeatherFlyout { get; set; }
    
    private bool CanDoLoadWeatherReports(object obj) => true;

    private async void DoLoadWeatherReports(object obj)
    {
        await WeatherService.GetReports(WeatherReports, ExtendedWeatherReports);
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
        await WeatherService.RemoveCity(WeatherReports, ExtendedWeatherReports, SelectedIndex);
        Status = WeatherService.Status;
    }

    private bool CanShowWeatherFlyout(object obj) => true;
 
    private void DoShowWeatherFlyout(object obj)
    {
        IsShadowRectangleEnabled = true;
        ShadowRectangleOpacity = FlyoutOpenOpacity;
        FlyoutMargin = new Thickness(FlyoutOpenMargin, 0, 0, 0);
    }
    
    private bool CanHideWeatherFlyout(object obj) => true;

    private void DoHideWeatherFlyout(object obj)
    {
        IsShadowRectangleEnabled = false;
        ShadowRectangleOpacity = FlyoutClosedOpacity;
        FlyoutMargin = new Thickness(FlyoutClosedMargin, 0, 0, 0);
    }
}