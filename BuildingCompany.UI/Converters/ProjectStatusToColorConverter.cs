using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace BuildingCompany.UI.Converters;

public class ProjectStatusToColorConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string projectStatus) {
            return projectStatus.ToLower() switch
            {
                "cancelled" => Colors.DarkRed,
                "completed" => Colors.ForestGreen,
                "planned" => Colors.Orange,
                "inprogress" => Colors.Yellow,
                "created" => Colors.Gray,
                _ => Colors.Gray
            };
        }
        return Colors.Gray;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}