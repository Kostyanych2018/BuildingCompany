using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using BuildingCompany.Domain.Entities;
using BuildingCompany.Domain.Strategies;
using MongoDB.Bson;

namespace BuildingCompany.Application.Services;

public class ProjectTaskService(
    IUnitOfWork unitOfWork,
    IQualificationStrategy qualificationStrategy) : IProjectTaskService
{
    public async Task<ProjectTaskDto> CreateTask(ProjectTaskDto projectTaskDto)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(projectTaskDto.ProjectId);
        if (project == null)
            throw new ArgumentException("Проект не найден", nameof(projectTaskDto.ProjectId));
        
        var task = new ProjectTask(
            projectTaskDto.Name,
            projectTaskDto.Description,
            projectTaskDto.ProjectId,
            projectTaskDto.RequiredPosition,
            projectTaskDto.RequiredExperience,
            projectTaskDto.RequiredCertificationLevel
        );
        await unitOfWork.ProjectTaskRepository.AddAsync(task);
        await unitOfWork.SaveAllAsync();
        return task.ToDto();
    }

    public async Task<ProjectTaskDto?> GetTask(ObjectId id)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(id);
        return task?.ToDto();
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetTasksByProject(ObjectId projectId)
    {
        var allTasks = await unitOfWork.ProjectTaskRepository.GetAllAsync();
        var tasks = allTasks.Where(t => t.ProjectId == projectId);
        return tasks.ToDto();
    }

    public async Task<bool> UpdateTask(ProjectTaskDto projectTaskDto)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(projectTaskDto.Id);
        if (task == null) return false;
        
        task.UpdateDetails(
            projectTaskDto.Name, 
            projectTaskDto.Description, 
            projectTaskDto.RequiredPosition,
            projectTaskDto.RequiredExperience,
            projectTaskDto.RequiredCertificationLevel
        );
        
        task.UpdateCompletionPercentage(projectTaskDto.CompletionPercentage);
        
        if (Enum.TryParse(projectTaskDto.Status, out ProjectTaskStatus status)) {
            task.UpdateStatus(status);
        }

        await unitOfWork.ProjectTaskRepository.UpdateAsync(task);
        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> DeleteTask(ObjectId id)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(id);
        if (task == null) return false;
        await unitOfWork.ProjectTaskRepository.DeleteAsync(task);
        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> AssignEmployeeToTask(ObjectId taskId, ObjectId employeeId)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null) return false;
        
        var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(employeeId);
        if (employee is not { Status: EmployeeStatus.Available }) return false;
        
        if (!qualificationStrategy.IsEmployeeQualified(employee, task))
            return false;

        task.AssignEmployee(employee);
        employee.AssignTask(task);
        employee.SetStatus(EmployeeStatus.Busy);
        await unitOfWork.ProjectTaskRepository.UpdateAsync(task);
        await unitOfWork.EmployeesRepository.UpdateAsync(employee);
        await unitOfWork.SaveAllAsync();
        return true;
    }
    
    public async Task<IEnumerable<EmployeeDto>> GetQualifiedEmployees(ObjectId taskId)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null) return [];
        
        var allEmployees = await unitOfWork.EmployeesRepository.GetAllAsync(e => e.Status == EmployeeStatus.Available);
        
        var qualifiedEmployees = allEmployees
            .Where(employee => qualificationStrategy.IsEmployeeQualified(employee, task))
            .ToList();
        
        return qualifiedEmployees.ToDto();
    }
}