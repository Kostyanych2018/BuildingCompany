using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.Converters;

public class IntToProgressConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int percentage)
        {
            return percentage / 100.0d;
        }
        return 0.0;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double progress)
        {
            return (int)(progress * 100.0);
        }
        return 0;
    }
}