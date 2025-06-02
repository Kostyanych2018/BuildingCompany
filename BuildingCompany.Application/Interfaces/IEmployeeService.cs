using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployees();
    Task<EmployeeDto?> GetEmployee(ObjectId id);
    Task<EmployeeDto> CreateEmployee(EmployeeDto employeeDto);
    Task<bool> UpdateEmployee(EmployeeDto employeeDto);
    Task<bool> DeleteEmployee(ObjectId id);
    Task<ProjectTaskDto?> GetAssignedTask(ObjectId employeeId);
}