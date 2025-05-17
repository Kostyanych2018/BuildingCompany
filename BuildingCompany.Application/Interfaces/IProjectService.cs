using BuildingCompany.Application.DTOs;

namespace BuildingCompany.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProject(ProjectDto projectDto);
    Task<ProjectDto?> GetProject(int id);
    Task<IEnumerable<ProjectDto>> GetProjects();
    Task<bool> UpdateProject(int id,ProjectDto projectDto);
    Task<bool> DeleteProject(int id);
}