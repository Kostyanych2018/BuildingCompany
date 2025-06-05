using MongoDB.Bson;

namespace BuildingCompany.Domain.Entities;

public class TaskMaterialRequirement : Entity, IEquatable<TaskMaterialRequirement>
{
    public ObjectId TaskId { get; set; }
    public ObjectId MaterialId { get; set; }
    public int RequiredQuantity { get; set; }
    public bool IsFulfilled { get; set; }
    
    public TaskMaterialRequirement() { }
    
    public TaskMaterialRequirement(ObjectId taskId, ObjectId materialId, int requiredQuantity)
    {
        if (requiredQuantity <= 0)
            throw new ArgumentException("Требуемое количество должно быть положительным числом.", nameof(requiredQuantity));
            
        TaskId = taskId;
        MaterialId = materialId;
        RequiredQuantity = requiredQuantity;
    }
    
    
    public void UpdateRequiredQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Требуемое количество должно быть положительным числом.", nameof(newQuantity));
            
        RequiredQuantity = newQuantity;
    }
    
    public bool Equals(TaskMaterialRequirement? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TaskId == other.TaskId &&
               MaterialId == other.MaterialId &&
               RequiredQuantity == other.RequiredQuantity;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TaskMaterialRequirement)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TaskId, MaterialId, RequiredQuantity);
    }
} 