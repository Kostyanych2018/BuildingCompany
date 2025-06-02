using System.Globalization;

namespace BuildingCompany.UI.Converters;

public class BudgetStatusConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isBudgetAvailable)
        {
            return isBudgetAvailable ? "Достаточно средств" : "Недостаточно средств";
        }
        
        return "Неизвестно";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }   
}