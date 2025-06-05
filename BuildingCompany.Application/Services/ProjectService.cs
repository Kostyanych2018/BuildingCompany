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

public class ProjectService(IUnitOfWork unitOfWork) : IProjectService
{
 
    public async Task<ProjectDto> CreateProject(ProjectDto projectDto)
    {
        var project = new Project(
            projectDto.Name,
            projectDto.Description,
            projectDto.Budget
        );
        await unitOfWork.ProjectsRepository.AddAsync(project);
        await unitOfWork.SaveAllAsync();
        return project.ToDto();
    }

    public async Task<ProjectDto?> GetProject(ObjectId id)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(id);
        return project?.ToDto();
    }

    public async Task<IEnumerable<ProjectDto>> GetProjects()
    {
        var projects = await unitOfWork.ProjectsRepository.GetAllAsync();
        return projects.ToDto();
    }

    public async Task<bool> UpdateProject(ProjectDto projectDto)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(projectDto.Id);
        if (project == null) return false;

        project.UpdateProject(projectDto.Name, projectDto.Description, projectDto.Budget);
        var newStatus = Enum.TryParse(projectDto.Status, out ProjectStatus status);
        if (newStatus) {
            project.SetStatus(status);
        }

        await unitOfWork.ProjectsRepository.UpdateAsync(project);
        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> DeleteProject(ObjectId id)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(id);
        if (project == null) return false;

        await unitOfWork.ProjectsRepository.DeleteAsync(project);
        await unitOfWork.SaveAllAsync();
        return true;
    }
    
    public async Task<bool> CompleteProject(ObjectId projectId)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(projectId);
        if (project == null) return false;
        
        var tasks = await unitOfWork.ProjectTaskRepository.GetAllAsync(t => t.ProjectId == projectId);
        var tasksList = tasks.ToList();
        
        if (tasksList.Any(t => t.CompletionPercentage < 100))
        {
            return false; 
        }
        
        project.SetStatus(ProjectStatus.Completed);
        await unitOfWork.ProjectsRepository.UpdateAsync(project);
        
        foreach (var task in tasksList.Where(t => t.Status != ProjectTaskStatus.Completed))
        {
            task.UpdateStatus(ProjectTaskStatus.Completed);
            await unitOfWork.ProjectTaskRepository.UpdateAsync(task);
            
            if (task.AssignedEmployeeId.HasValue)
            {
                var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(task.AssignedEmployeeId.Value);
                if (employee != null)
                {
                    employee.SetStatus(EmployeeStatus.Available);
                    employee.AssignedTaskId = null;
                    await unitOfWork.EmployeesRepository.UpdateAsync(employee);
                }
            }
        }
        
        var requirements = await unitOfWork.TaskMaterialRequirementRepository.GetAllAsync(
            r => tasksList.Any(t => t.Id == r.TaskId));
        
        foreach (var requirement in requirements)
        {
            await unitOfWork.TaskMaterialRequirementRepository.DeleteAsync(requirement);
        }
        
        await unitOfWork.SaveAllAsync();
        return true;
    }
    
    public async Task<bool> UpdateProjectStatus(ObjectId projectId)
    {
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(projectId);
        if (project == null) return false;
        
        var tasks = await unitOfWork.ProjectTaskRepository.GetAllAsync(t => t.ProjectId == projectId);
        var tasksList = tasks.ToList();
        
        if (tasksList.Count == 0)
        {
            if (project.Status != ProjectStatus.Created)
            {
                project.SetStatus(ProjectStatus.Created);
                await unitOfWork.ProjectsRepository.UpdateAsync(project);
                await unitOfWork.SaveAllAsync();
            }
            return true;
        }
        
        if (tasksList.All(t => t.CompletionPercentage == 100))
        {
            if (project.Status != ProjectStatus.Completed)
            {
                project.SetStatus(ProjectStatus.Completed);
                await unitOfWork.ProjectsRepository.UpdateAsync(project);
                await unitOfWork.SaveAllAsync();
            }
            return true;
        }
        
        if (tasksList.Any(t => t.Status == ProjectTaskStatus.InProgress || t.CompletionPercentage > 0))
        {
            if (project.Status != ProjectStatus.InProgress)
            {
                project.SetStatus(ProjectStatus.InProgress);
                await unitOfWork.ProjectsRepository.UpdateAsync(project);
                await unitOfWork.SaveAllAsync();
            }
            return true;
        }
        
        if (project.Status != ProjectStatus.Planned)
        {
            project.SetStatus(ProjectStatus.Planned);
            await unitOfWork.ProjectsRepository.UpdateAsync(project);
            await unitOfWork.SaveAllAsync();
        }
        
        return true;
    }
}