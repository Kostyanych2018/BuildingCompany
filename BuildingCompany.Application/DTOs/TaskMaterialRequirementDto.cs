using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class TaskMaterialRequirementDto
{
    public ObjectId Id { get; set; }
    public ObjectId TaskId { get; set; }
    public ObjectId MaterialId { get; set; }
    public int RequiredQuantity { get; set; }
    public bool IsFulfilled { get; set; }
    
    // Navigation properties
    public MaterialDto? Material { get; set; }
    
    // Computed properties
    public decimal TotalCost => Material?.UnitPrice * RequiredQuantity ?? 0;
    public string? TotalCostDisplay => Material != null ? $"{TotalCost:N2} BYN" : null;
    public string? AvailabilityStatus => Material != null 
        ? (Material.Quantity >= RequiredQuantity ? "Доступно" : "Недостаточно") 
        : "Неизвестно";
    public string? RequirementDisplay => Material != null 
        ? $"{RequiredQuantity} {Material.UnitOfMeasure}" 
        : $"{RequiredQuantity}";
} 