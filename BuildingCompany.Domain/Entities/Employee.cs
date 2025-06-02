using System;
using MongoDB.Bson;

namespace BuildingCompany.Domain.Entities;

public class Employee : Entity, IEquatable<Employee>
{
    public string FullName { get; set; } = null!;
    public string Position { get; set; } = null!;
    public EmployeeStatus Status { get; set; }
    public int Experience { get; set; } // Опыт работы в годах
    public int CertificationLevel { get; set; } // Уровень сертификации от 1 до 5

    public ObjectId? AssignedTaskId { get; set; }

    public Employee() { }

    public Employee(string fullName, string position, int experience = 0, int certificationLevel = 1,
        EmployeeStatus status = EmployeeStatus.Available)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Имя сотрудника не может быть пустым.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(position));
        if (experience < 0)
            throw new ArgumentException("Опыт работы не может быть отрицательным.", nameof(experience));
        if (certificationLevel < 1 || certificationLevel > 5)
            throw new ArgumentException("Уровень сертификации должен быть от 1 до 5.", nameof(certificationLevel));

        FullName = fullName;
        Position = position;
        Status = status;
        Experience = experience;
        CertificationLevel = certificationLevel;
    }

    public void UpdateDetails(string fullName, string position, int experience, int certificationLevel)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Имя сотрудника не может быть пустым.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(position));
        if (experience < 0)
            throw new ArgumentException("Опыт работы не может быть отрицательным.", nameof(experience));
        if (certificationLevel < 1 || certificationLevel > 5)
            throw new ArgumentException("Уровень сертификации должен быть от 1 до 5.", nameof(certificationLevel));
            
        FullName = fullName;
        Position = position;
        Experience = experience;
        CertificationLevel = certificationLevel;
    }

    public void SetStatus(EmployeeStatus status)
    {
        if (Status != status)
            Status = status;
    }

    public void AssignTask(ProjectTask task)
    {
        AssignedTaskId = task.Id;
    }

    public bool Equals(Employee? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return FullName == other.FullName && 
               Position == other.Position && 
               Status == other.Status && 
               Experience == other.Experience &&
               CertificationLevel == other.CertificationLevel &&
               AssignedTaskId == other.AssignedTaskId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Employee)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FullName, Position, (int)Status, Experience, CertificationLevel, AssignedTaskId);
    }
}

public enum EmployeeStatus
{
    Available,
    Busy
}