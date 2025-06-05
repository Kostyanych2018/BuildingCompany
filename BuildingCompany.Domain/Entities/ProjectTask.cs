using System;
using MongoDB.Bson;

namespace BuildingCompany.Domain.Entities;

public class ProjectTask : Entity, IEquatable<ProjectTask>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public int CompletionPercentage { get; set; }
    public ObjectId ProjectId { get; set; }
    public ObjectId? AssignedEmployeeId { get; set; }
    
    public string? RequiredPosition { get; set; }
    public int? RequiredExperience { get; set; }
    public int? RequiredCertificationLevel { get; set; }

    public ProjectTask() { }

    public ProjectTask(string name, string? description, ObjectId projectId, 
        string? requiredPosition = null, int? requiredExperience = null, int? requiredCertificationLevel = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название задачи не может быть пустым.", nameof(name));

        if (requiredCertificationLevel.HasValue && (requiredCertificationLevel < 1 || requiredCertificationLevel > 5))
            throw new ArgumentException("Требуемый уровень сертификации должен быть от 1 до 5.", nameof(requiredCertificationLevel));

        if (requiredExperience.HasValue && requiredExperience < 0)
            throw new ArgumentException("Требуемый опыт работы не может быть отрицательным.", nameof(requiredExperience));

        Name = name;
        Description = description;
        ProjectId = projectId;
        Status = ProjectTaskStatus.Created;
        CompletionPercentage = 0;
        RequiredPosition = requiredPosition;
        RequiredExperience = requiredExperience;
        RequiredCertificationLevel = requiredCertificationLevel;
    }

    public void UpdateDetails(string name, string? description, 
        string? requiredPosition, int? requiredExperience, int? requiredCertificationLevel)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название задачи не может быть пустым.", nameof(name));

        if (requiredCertificationLevel.HasValue && (requiredCertificationLevel < 1 || requiredCertificationLevel > 5))
            throw new ArgumentException("Требуемый уровень сертификации должен быть от 1 до 5.", nameof(requiredCertificationLevel));

        if (requiredExperience.HasValue && requiredExperience < 0)
            throw new ArgumentException("Требуемый опыт работы не может быть отрицательным.", nameof(requiredExperience));

        Name = name;
        Description = description;
        RequiredPosition = requiredPosition;
        RequiredExperience = requiredExperience;
        RequiredCertificationLevel = requiredCertificationLevel;
    }

    public void UpdateStatus(ProjectTaskStatus status)
    {
        Status = status;
    }

    public void UpdateCompletionPercentage(int percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Процент выполнения должен быть от 0 до 100.", nameof(percentage));

        CompletionPercentage = percentage;
    }

    public void AssignEmployee(Employee employee)
    {
        AssignedEmployeeId = employee.Id;
    }

    public bool Equals(ProjectTask? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && 
               Description == other.Description && 
               Status == other.Status && 
               CompletionPercentage == other.CompletionPercentage && 
               ProjectId == other.ProjectId && 
               AssignedEmployeeId == other.AssignedEmployeeId && 
               RequiredPosition == other.RequiredPosition &&
               RequiredExperience == other.RequiredExperience &&
               RequiredCertificationLevel == other.RequiredCertificationLevel;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ProjectTask)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name);
        hashCode.Add(Description);
        hashCode.Add((int)Status);
        hashCode.Add(CompletionPercentage);
        hashCode.Add(ProjectId);
        hashCode.Add(AssignedEmployeeId);
        hashCode.Add(RequiredPosition);
        hashCode.Add(RequiredExperience);
        hashCode.Add(RequiredCertificationLevel);
        return hashCode.ToHashCode();
    }
    
}

public enum ProjectTaskStatus
{
    Created,
    InProgress,
    Paused,
    Completed,
}