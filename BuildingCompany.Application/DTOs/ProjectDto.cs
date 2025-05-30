using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class ProjectDto
{
    public ObjectId Id { get; set; }
    public string Name { get;  set; }
    public string? Description { get; set; }
    public decimal Budget { get;  set; }
    public string Status { get;  set; }
    
}