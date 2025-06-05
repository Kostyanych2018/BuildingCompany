using MongoDB.Bson;

namespace BuildingCompany.Application.DTOs;

public class EmployeeDto
{
    public ObjectId Id { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public string Status { get; set; }
    public int Experience { get; set; }
    public int CertificationLevel { get; set; }
    public ObjectId? AssignedTaskId { get; set; }
    
    public string ImagePath { get; set; } = "employee_default.png";
}