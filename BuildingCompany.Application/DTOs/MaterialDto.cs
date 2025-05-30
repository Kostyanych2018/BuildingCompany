using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class MaterialDto
{
    public ObjectId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    
    public string DisplayNameWithUnit => $"{Name} ({UnitOfMeasure})";
    public string StockDisplay => $"{Quantity} {UnitOfMeasure}";
    public string PriceDisplay => $" {UnitPrice} BYN / {UnitOfMeasure}"; 
}