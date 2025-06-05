using BuildingCompany.Domain.Abstractions;

namespace BuildingCompany.Domain.MaterialPricing;

public class PremiumMaterialPricing : MaterialPricingDecorator
{
    private const decimal PremiumSurcharge = 0.20m; 
    
    public PremiumMaterialPricing(IMaterialPricing materialPricing) 
        : base(materialPricing)
    {
    }

    public override decimal CalculatePrice(decimal basePrice)
    {
        decimal baseCalculation = base.CalculatePrice(basePrice);
        return baseCalculation * (1 + PremiumSurcharge);
    }

    public override string Description => $"{base.Description} + Премиум надбавка (20%)";
} 