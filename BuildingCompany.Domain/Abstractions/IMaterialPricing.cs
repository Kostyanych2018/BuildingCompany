namespace BuildingCompany.Domain.Abstractions;

/// <summary>
/// Interface for material pricing strategy
/// </summary>
public interface IMaterialPricing
{
    /// <summary>
    /// Calculate the price of the material
    /// </summary>
    /// <param name="basePrice">Base price of the material</param>
    /// <returns>Final price after applying pricing strategy</returns>
    decimal CalculatePrice(decimal basePrice);
    
    /// <summary>
    /// Description of the pricing adjustment
    /// </summary>
    string Description { get; }
} 