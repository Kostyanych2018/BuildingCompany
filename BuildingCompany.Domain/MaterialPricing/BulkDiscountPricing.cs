using BuildingCompany.Domain.Abstractions;

namespace BuildingCompany.Domain.MaterialPricing;

public class BulkDiscountPricing : MaterialPricingDecorator
{
    private const decimal BulkDiscount = 0.10m;
    
    public BulkDiscountPricing(IMaterialPricing materialPricing) 
        : base(materialPricing)
    {
    }

    public override decimal CalculatePrice(decimal basePrice)
    {
        decimal baseCalculation = base.CalculatePrice(basePrice);
        return baseCalculation * (1 - BulkDiscount);
    }

    public override string Description => $"{base.Description} - Оптовая скидка (10%)";
} 