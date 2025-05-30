using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
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
}