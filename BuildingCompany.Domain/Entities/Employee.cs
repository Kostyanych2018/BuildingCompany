namespace BuildingCompany.Domain.Entities;

public class Employee : Entity, IEquatable<Employee>
{
    public string FullName { get; set; } = null!;
    public string Position { get; set; } = null!;
    public EmployeeStatus Status { get; set; }

    public int? AssignedTaskId { get; set; }

    public Employee() { }

    public Employee(string fullName, string position,
        EmployeeStatus status = EmployeeStatus.Available)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Имя сотрудника не может быть пустым.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(position));

        FullName = fullName;
        Position = position;
        Status = status;
    }

    public void UpdateDetails(string fullName, string position)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Имя сотрудника не может быть пустым.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(position));
        FullName = fullName;
        Position = position;
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
        return HashCode.Combine(FullName, Position, (int)Status, AssignedTaskId);
    }
}

public enum EmployeeStatus
{
    Available,
    Busy
}