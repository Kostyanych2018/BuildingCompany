using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace BuildingCompany.UI.Converters;

public class NumericValidationBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    private void OnEntryTextChanged(object? sender, TextChangedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.NewTextValue))
            return;

        bool isValid = int.TryParse(args.NewTextValue, out int result) && result > 0;

        if (!isValid)
            ((Entry)sender).Text = args.OldTextValue;
    }
} 