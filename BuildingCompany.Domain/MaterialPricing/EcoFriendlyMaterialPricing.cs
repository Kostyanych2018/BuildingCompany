using BuildingCompany.Domain.Abstractions;

namespace BuildingCompany.Domain.MaterialPricing;

public class EcoFriendlyMaterialPricing : MaterialPricingDecorator
{
    private const decimal EcoFriendlySurcharge = 0.15m; 
    
    public EcoFriendlyMaterialPricing(IMaterialPricing materialPricing) 
        : base(materialPricing)
    {
    }

    public override decimal CalculatePrice(decimal basePrice)
    {
        decimal baseCalculation = base.CalculatePrice(basePrice);
        return baseCalculation * (1 + EcoFriendlySurcharge);
    }

    public override string Description => $"{base.Description} + Эко надбавка (15%)";
} 