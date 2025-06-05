using BuildingCompany.Domain.Abstractions;

namespace BuildingCompany.Domain.MaterialPricing;


public abstract class MaterialPricingDecorator : IMaterialPricing
{
    protected readonly IMaterialPricing _materialPricing;

    protected MaterialPricingDecorator(IMaterialPricing materialPricing)
    {
        _materialPricing = materialPricing;
    }

    public virtual decimal CalculatePrice(decimal basePrice)
    {
        return _materialPricing.CalculatePrice(basePrice);
    }

    public virtual string Description => _materialPricing.Description;
} 