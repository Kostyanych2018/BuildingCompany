using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;

namespace BuildingCompany.Application.Services;

public class ProjectService(IRepository<Project> projectRepository) : IProjectService
{
    public async Task<ProjectDto> CreateProject(ProjectDto projectDto)
    {
        var project = new Project(
            projectDto.Name,
            projectDto.Description,
            projectDto.Budget
        );
        await projectRepository.AddAsync(project);
        return project.ToDto();
    }

    public async Task<ProjectDto?> GetProject(int id)
    {
        var project = await projectRepository.GetByIdAsync(id);
        return project?.ToDto();
    }

    public async Task<IEnumerable<ProjectDto>> GetProjects()
    {
        var projects = await projectRepository.GetAllAsync();
        return projects.ToDto();
    }

    public async Task<bool> UpdateProject(int id, ProjectDto projectDto)
    {
        var project = await projectRepository.GetByIdAsync(id);
        if (project == null) return false;

        project.UpdateProject(projectDto.Name, projectDto.Description, projectDto.Budget);
        var newStatus = Enum.TryParse(projectDto.Status, out ProjectStatus status);
        if (newStatus) {
            project.SetStatus(status);
        }

        await projectRepository.UpdateAsync(project);
        return true;
    }

    public async Task<bool> DeleteProject(int id)
    {
        var project = await projectRepository.GetByIdAsync(id);
        if (project == null) return false;

        await projectRepository.DeleteAsync(project);
        return true;
    }
}