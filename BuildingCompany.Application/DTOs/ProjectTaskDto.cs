using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class ProjectTaskDto
{
    public ObjectId Id { get; set; }
    public string Name { get;  set; }
    public string? Description { get; set; }
    public string Status { get;  set; }
    public int CompletionPercentage { get;  set; }
    public ObjectId ProjectId { get;  set; }
    public ObjectId? AssignedEmployeeId { get;  set; }
}