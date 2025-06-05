using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class IntToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue / 100.0;
        }
        
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            return (int)(doubleValue * 100);
        }
        
        return 0;
    }
} 