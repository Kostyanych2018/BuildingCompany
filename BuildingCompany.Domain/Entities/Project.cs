namespace BuildingCompany.Domain.Entities;

public class Project : Entity, IEquatable<Project>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Budget { get; set; }
    public ProjectStatus Status { get; set; }
    public List<ProjectTask> Tasks { get; set; }= [];

    public Project() { }

    public Project(string name, string? description, decimal budget)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя проекта не может быть пустым.", nameof(name));
        if (budget < 0)
            throw new ArgumentException("Бюджет не может быть отрицательным", nameof(budget));
        Name = name;
        Description = description;
        Budget = budget;
        Status = ProjectStatus.Created;
    }

    public void UpdateProject(string name, string? description, decimal budget)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя проекта не может быть пустым.", nameof(name));
        if (budget < 0)
            throw new ArgumentException("Бюджет не может быть отрицательным", nameof(budget));

        Name = name;
        Description = description;
        Budget = budget;
    }

    public void SetStatus(ProjectStatus status)
    {
        if (Status != status)
            Status = status;
    }

    public bool Equals(Project? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Id == other.Id &&
               Name == other.Name && Description == other.Description
               && Budget == other.Budget
               && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Project)obj);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(Budget);
        hash.Add((int)Status);
        return hash.ToHashCode();
    }
}

public enum ProjectStatus
{
    Created,
    Planned,
    InProgress,
    Completed,
    Cancelled
}