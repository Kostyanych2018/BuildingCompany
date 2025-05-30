using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProject(ProjectDto projectDto);
    Task<IEnumerable<ProjectDto>> GetProjects();
    Task<ProjectDto?> GetProject(ObjectId id);
    Task<bool> UpdateProject(ProjectDto projectDto);
    Task<bool> DeleteProject(ObjectId id);
}