using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class ImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string imageName && !string.IsNullOrEmpty(imageName))
        {
            try
            {
                return ImageSource.FromFile(imageName);
            }
            catch
            {
                return ImageSource.FromFile("dotnet_bot.png");
            }
        }
        
        return ImageSource.FromFile("dotnet_bot.png");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 