using System;

namespace AvaloniaWeatherTracker.Services;

public static class DateTimeFormatter
{
    public static string FormatToString(DateTime time) =>
        $"{time.Year}.{EnsureTwoDigits(time.Month)}.{EnsureTwoDigits(time.Day)} " +
        $"{EnsureTwoDigits(time.Hour)}:{EnsureTwoDigits(time.Minute)}:{EnsureTwoDigits(time.Second)}";

    private static string EnsureTwoDigits(int value) => value < 10 ? $"0{value}" : value.ToString();
}