using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using AvaloniaWeatherTracker.Commands;
using AvaloniaWeatherTracker.Models;
using AvaloniaWeatherTracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaWeatherTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedIndex))]
    [NotifyPropertyChangedFor(nameof(SelectedExtendedWeatherReport))]    
    private ObservableCollection<ExtendedWeatherReport> _extendedWeatherReports;

    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private string _textBotText;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedExtendedWeatherReport))]
    [NotifyPropertyChangedFor(nameof(ReportSelected))]    
    private int _selectedIndex;

    [ObservableProperty]
    private double _shadowRectangleOpacity;

    [ObservableProperty]
    private double _flyoutOpacity;

    [ObservableProperty]
    private bool _isShadowRectangleEnabled;

    [ObservableProperty]
    private bool _isFlyoutEnabled;

    private const double FlyoutClosedOpacity = 0;
    private const double FlyoutOpenOpacity = 0.6;
    
    public MainViewModel()
    {
        ExtendedWeatherReports = new ObservableCollection<ExtendedWeatherReport>();
        
        LoadReports = new RelayCommand(DoLoadWeatherReports, CanDoLoadWeatherReports);
        AddCity = new RelayCommand(DoAddCity, CanDoAddCity);
        RemoveCity = new RelayCommand(DoRemoveCity, CanDoRemoveCity);
        ShowWeatherFlyout = new RelayCommand(DoShowWeatherFlyout, CanShowWeatherFlyout);
        HideWeatherFlyout = new RelayCommand(DoHideWeatherFlyout, CanHideWeatherFlyout);

        _status = "Last fetched at: never fetched!";
        _textBotText = "";
        _shadowRectangleOpacity = FlyoutClosedOpacity;
        _isShadowRectangleEnabled = false;
        _isFlyoutEnabled = false;
        _flyoutOpacity = 0;
    }

    public ExtendedWeatherReport? SelectedExtendedWeatherReport
    {
        get
        {
            try
            {
                return ExtendedWeatherReports[SelectedIndex];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public bool ReportSelected => SelectedExtendedWeatherReport != null;

    public ICommand LoadReports { get; set; }
    public ICommand AddCity { get; set; }
    public ICommand RemoveCity { get; set; }
    public ICommand ShowWeatherFlyout { get; set; }  
    public ICommand HideWeatherFlyout { get; set; }
    
    private bool CanDoLoadWeatherReports(object obj) => true;

    private async void DoLoadWeatherReports(object obj)
    {
        await WeatherService.GetReports(ExtendedWeatherReports);
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
        await WeatherService.RemoveCity(ExtendedWeatherReports, SelectedIndex);
        Status = WeatherService.Status;
    }

    private bool CanShowWeatherFlyout(object obj) => true;
 
    private void DoShowWeatherFlyout(object obj)
    {
        IsShadowRectangleEnabled = true;
        IsFlyoutEnabled = true;
        ShadowRectangleOpacity = FlyoutOpenOpacity;
        FlyoutOpacity = 1;
    }
    
    private bool CanHideWeatherFlyout(object obj) => true;

    private void DoHideWeatherFlyout(object obj)
    {
        IsShadowRectangleEnabled = false;
        IsFlyoutEnabled = false;
        ShadowRectangleOpacity = FlyoutClosedOpacity;
        FlyoutOpacity = 0;
    }
}