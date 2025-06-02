using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class EmployeeStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "Available" => Colors.Green,
                "Busy" => Colors.Orange,
                _ => Colors.Gray
            };
        }
        
        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 