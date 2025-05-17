using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;

namespace BuildingCompany.Application.Services;

public class EmployeeService(IRepository<Employee> employeeRepository, 
    IRepository<ProjectTask> taskRepository): IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        var employees = await employeeRepository.GetAllAsync();
        return employees.ToDto();

    }
    public async Task<EmployeeDto?> GetEmployee(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        return employee?.ToDto();
    }

    public async Task<ProjectTaskDto?> GetAssignedTask(int employeeId)
    {
        var employee = await employeeRepository.GetByIdAsync(employeeId);
        if (employee == null) return null;
        var task = (await taskRepository.GetAllAsync())
            .FirstOrDefault(t=>t.AssignedEmployeeId==employeeId);
        return task?.ToDto();
    }
}