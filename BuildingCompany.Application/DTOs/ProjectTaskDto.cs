namespace BuildingCompany.Application.DTOs;

public class ProjectTaskDto
{
    public int Id { get; set; }
    public string Name { get;  set; }
    public string? Description { get; set; }
    public string Status { get;  set; }
    public int CompletionPercentage { get;  set; }
    public int ProjectId { get;  set; }
    public int? AssignedEmployeeId { get;  set; }
}