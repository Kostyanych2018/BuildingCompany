using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using BuildingCompany.Domain.Entities;
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

    public async Task<EmployeeDto> CreateEmployee(EmployeeDto employeeDto)
    {
        var employee = new Employee(
            employeeDto.FullName,
            employeeDto.Position,
            employeeDto.Experience,
            employeeDto.CertificationLevel
        )
        {
            Status = EmployeeStatus.Available
        };
        
        await unitOfWork.EmployeesRepository.AddAsync(employee);
        await unitOfWork.SaveAllAsync();
        return employee.ToDto();
    }

    public async Task<bool> UpdateEmployee(EmployeeDto employeeDto)
    {
        var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(employeeDto.Id);
        if (employee == null) return false;

        employee.UpdateDetails(
            employeeDto.FullName, 
            employeeDto.Position,
            employeeDto.Experience,
            employeeDto.CertificationLevel
        );
        
        if (Enum.TryParse(employeeDto.Status, out EmployeeStatus status))
        {
            employee.SetStatus(status);
        }

        await unitOfWork.EmployeesRepository.UpdateAsync(employee);
        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> DeleteEmployee(ObjectId id)
    {
        var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(id);
        if (employee == null) return false;
        
        if (employee.AssignedTaskId.HasValue)
        {
            return false;
        }
        
        await unitOfWork.EmployeesRepository.DeleteAsync(employee);
        await unitOfWork.SaveAllAsync();
        return true;
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