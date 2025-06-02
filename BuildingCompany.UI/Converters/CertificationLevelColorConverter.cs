using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class CertificationLevelColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int currentLevel || parameter is not string paramLevel || !int.TryParse(paramLevel, out int buttonLevel))
            return Colors.Gray;

        return currentLevel == buttonLevel ? Colors.Green : Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 