using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class AvailabilityStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "Доступно" => Colors.Green,
                "Недостаточно" => Colors.Red,
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