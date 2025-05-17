namespace BuildingCompany.Application.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public string Status { get; set; }
    public int? AssignedTaskId { get; set; }
}