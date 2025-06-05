using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace BuildingCompany.UI.Converters;

public class TaskStatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        while (true) {
            if (value is ProjectTaskStatus status) {
                return status switch
                {
                    ProjectTaskStatus.Created => Colors.Gray,
                    ProjectTaskStatus.InProgress => Colors.Orange,
                    ProjectTaskStatus.Paused => Colors.DarkOrange,
                    ProjectTaskStatus.Completed => Colors.Green,
                    _ => Colors.Black
                };
            }

            if (value is string statusStr) {
                if (Enum.TryParse<ProjectTaskStatus>(statusStr, out var result)) {
                    value = result;
                    continue;
                }
            }
            break;
        }

        return Colors.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}