using System;
using System.Windows.Input;

namespace AvaloniaWeatherTracker.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
    {
        ExecuteMethod = executeMethod;
        CanExecuteMethod = canExecuteMethod;
    }
        
    public Action<object> ExecuteMethod { get; set; }
    public Predicate<object> CanExecuteMethod { get; set; }

    public bool CanExecute(object parameter) => CanExecuteMethod(parameter);

    public void Execute(object parameter) => ExecuteMethod(parameter);
}