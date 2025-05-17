using BuildingCompany.Application.DTOs;

namespace BuildingCompany.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployees();
    Task<EmployeeDto?> GetEmployee(int id);
}