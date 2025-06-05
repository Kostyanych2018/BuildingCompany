using System.Collections.Generic;
using System.Linq;

namespace BuildingCompany.UI.Services;

public class ImageService
{
    private readonly Dictionary<string, List<string>> _employeeImages = new()
    {
        ["Инженер"] = new List<string> { "engineer1.png", "engineer2.png" },
        ["Менеджер"] = new List<string> { "manager1.png", "manager2.png" },
        ["Рабочий"] = new List<string> { "worker1.png", "worker2.png", },
        ["Архитектор"] = new List<string> { "architect1.png", "architect2.png" },
        ["default"] = new List<string> { "employee_default.png" }
    };

    private readonly Dictionary<string, List<string>> _materialImages = new()
    {
        ["Стандарт"] = new List<string> { "material_standard.png" },
        ["Премиум"] = new List<string> { "material_premium.png" },
        ["Эко"] = new List<string> { "material_eco.png" },
        ["default"] = new List<string> { "material_default.png" }
    };

    private readonly Dictionary<string, (string Primary, string Secondary)> _combinedCategories = new()
    {
        ["Премиум Эко"] = ("material_premium.png", "material_eco.png")
    };


    private readonly Random _random = new();

    public string GetEmployeeImage(string position)
    {
        if (_employeeImages.TryGetValue(position, out var images)) {
            return images[_random.Next(images.Count)];
        }

        return _employeeImages["default"][0];
    }

    public string GetMaterialImage(string category)
    {
        if (_combinedCategories.TryGetValue(category, out var combined)) {
            return combined.Primary;
        }

        if (_materialImages.TryGetValue(category, out var images)) {
            return images[0];
        }

        return _materialImages["default"][0];
    }

    public string GetSecondaryMaterialImage(string category)
    {
        if (_combinedCategories.TryGetValue(category, out var combined)) {
            return combined.Secondary;
        }

        return string.Empty;
    }

    public bool HasSecondaryImage(string category)
    {
        return _combinedCategories.ContainsKey(category);
    }
}