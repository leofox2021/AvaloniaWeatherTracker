using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaWeatherTracker.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaWeatherTracker;

public class ViewLocator : IDataTemplate
{
    public Control Build(object data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        
        return new TextBlock { Text = name };
    }

    public bool Match(object? data) => data is ObservableObject;
}