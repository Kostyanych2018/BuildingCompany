namespace BuildingCompany.Domain.Entities;

public class ProjectTask : Entity, IEquatable<ProjectTask>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public int CompletionPercentage { get; set; }
    public int ProjectId { get; set; }
    public int? AssignedEmployeeId { get; set; }
    
    public ProjectTask(){}

    public ProjectTask(string name, string? description,
        int projectId, int completionPercentage = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Название задачи не может быть пустым.");
        if (projectId <= 0) {
            throw new ArgumentOutOfRangeException(nameof(projectId), "ID проекта должно быть положительным");
        }
        
        Name = name;
        Description = description;
        Status = ProjectTaskStatus.Created;
        CompletionPercentage = completionPercentage;
        ProjectId = projectId;
        AssignedEmployeeId = null;
    }

    public void UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Название задачи не может быть пустым.");
        Name = name;
        Description = description;
    }

    public void UpdateCompletionPercentage(int percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Процент выполнения должен быть между 0 и 100.");
        CompletionPercentage = percentage;
        if (CompletionPercentage == 100) {
            Status = ProjectTaskStatus.Completed;
        }
    }

    public void SetStatus(ProjectTaskStatus status)
    {
        if (Status != status)
            Status = status;
    }

    public void AddToProject(int projectId)
    {
        if (projectId <= 0)
            throw new ArgumentOutOfRangeException(nameof(projectId),
                "ID проекта для добавления должен быть положительным.");
        ProjectId = projectId;
    }

    public void AssignEmployee(Employee employee)
    {
        if (employee.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(employee.Id), 
                "ID сотрудника должен быть положительным.");
        AssignedEmployeeId = employee.Id;
    }

    public bool Equals(ProjectTask? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Description == other.Description
                                  && Status == other.Status 
                                  && CompletionPercentage == other.CompletionPercentage
                                  && ProjectId == other.ProjectId
                                  && AssignedEmployeeId == other.AssignedEmployeeId;
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
        return HashCode.Combine(Name, Description, (int)Status, CompletionPercentage, ProjectId, AssignedEmployeeId);
    }
}

public enum ProjectTaskStatus
{
    Created,
    InProgress,
    Completed,
    Cancelled
}