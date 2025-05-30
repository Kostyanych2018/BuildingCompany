using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using MongoDB.Bson;

namespace BuildingCompany.Application.Services;

public class EmployeeService(IUnitOfWork unitOfWork): IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetEmployees()
    {
        var employees = await unitOfWork.EmployeesRepository.GetAllAsync();
        return employees.ToDto();

    }
    public async Task<EmployeeDto?> GetEmployee(ObjectId id)
    {
        var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(id);
        return employee?.ToDto();
    }

    public async Task<ProjectTaskDto?> GetAssignedTask(ObjectId employeeId)
    {
        var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(employeeId);
        if (employee == null) return null;
        var task = (await unitOfWork.ProjectTaskRepository.GetAllAsync())
            .FirstOrDefault(t=>t.AssignedEmployeeId==employeeId);
        return task?.ToDto();
    }
}