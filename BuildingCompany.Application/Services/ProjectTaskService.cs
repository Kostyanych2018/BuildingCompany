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
    IQualificationStrategy qualificationStrategy,
    IProjectService projectService) : IProjectTaskService
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
        
        await projectService.UpdateProjectStatus(projectTaskDto.ProjectId);
        
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
        
        await projectService.UpdateProjectStatus(task.ProjectId);
        
        return true;
    }

    public async Task<bool> DeleteTask(ObjectId id)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(id);
        if (task == null) return false;
        
        var projectId = task.ProjectId;
        
        await unitOfWork.ProjectTaskRepository.DeleteAsync(task);
        await unitOfWork.SaveAllAsync();
        
        // Обновляем статус проекта после удаления задачи
        await projectService.UpdateProjectStatus(projectId);
        
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
        
        // Если у сотрудника уже есть задача, освобождаем его
        if (employee.AssignedTaskId.HasValue)
        {
            var oldTask = await unitOfWork.ProjectTaskRepository.GetByIdAsync(employee.AssignedTaskId.Value);
            if (oldTask != null)
            {
                oldTask.AssignedEmployeeId = null;
                await unitOfWork.ProjectTaskRepository.UpdateAsync(oldTask);
                
                // Обновляем статус проекта для предыдущей задачи
                await projectService.UpdateProjectStatus(oldTask.ProjectId);
            }
        }
        
        // Если у задачи уже был назначен сотрудник, освобождаем его
        if (task.AssignedEmployeeId.HasValue)
        {
            var oldEmployee = await unitOfWork.EmployeesRepository.GetByIdAsync(task.AssignedEmployeeId.Value);
            if (oldEmployee != null)
            {
                oldEmployee.SetStatus(EmployeeStatus.Available);
                oldEmployee.AssignedTaskId = null;
                await unitOfWork.EmployeesRepository.UpdateAsync(oldEmployee);
            }
        }
        
        // Назначаем нового сотрудника на задачу
        task.AssignedEmployeeId = employee.Id;
        employee.SetStatus(EmployeeStatus.Busy);
        employee.AssignedTaskId = task.Id;
        
        await unitOfWork.ProjectTaskRepository.UpdateAsync(task);
        await unitOfWork.EmployeesRepository.UpdateAsync(employee);
        await unitOfWork.SaveAllAsync();
        
        // Обновляем статус проекта после назначения сотрудника
        await projectService.UpdateProjectStatus(task.ProjectId);
        
        return true;
    }

    public async Task<IEnumerable<EmployeeDto>> GetQualifiedEmployees(ObjectId taskId)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null)
            return Enumerable.Empty<EmployeeDto>();

        var allEmployees = await unitOfWork.EmployeesRepository.GetAllAsync(e => e.Status == EmployeeStatus.Available);
        var qualifiedEmployees = allEmployees.Where(e => qualificationStrategy.IsEmployeeQualified(e, task));
        
        return qualifiedEmployees.ToDto();
    }
}