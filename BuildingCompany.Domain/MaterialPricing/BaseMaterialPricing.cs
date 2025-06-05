using BuildingCompany.Domain.Abstractions;

namespace BuildingCompany.Domain.MaterialPricing;

/// <summary>
/// Base implementation for material pricing
/// </summary>
public class BaseMaterialPricing : IMaterialPricing
{
    public virtual decimal CalculatePrice(decimal basePrice)
    {
        return basePrice;
    }

    public virtual string Description => "Стандартная цена";
} 