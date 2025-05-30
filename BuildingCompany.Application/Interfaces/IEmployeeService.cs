using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployees();
    Task<EmployeeDto?> GetEmployee(ObjectId id);
    
}