using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class MaterialDto
{
    public ObjectId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; } = "Стандарт";
    
    public bool IsPremium { get; set; }
    public bool IsEcoFriendly { get; set; }
    public bool HasBulkDiscount { get; set; }
    
    public decimal FinalPrice { get; set; }
    public string PriceAdjustmentDescription { get; set; } = string.Empty;
    
    public string ImagePath { get; set; } = "material_default.png";
    
    public string SecondaryImagePath { get; set; } = string.Empty;
    
    public bool HasSecondaryImage => !string.IsNullOrEmpty(SecondaryImagePath);
    
    public string DisplayNameWithUnit => $"{Name} ({UnitOfMeasure})";
    public string StockDisplay => $"{Quantity} {UnitOfMeasure}";
    public string PriceDisplay => $" {UnitPrice} BYN / {UnitOfMeasure}";
    public string FinalPriceDisplay => $" {FinalPrice} BYN / {UnitOfMeasure}";
    public string CategoryDisplay => $"Категория: {Category}";
}