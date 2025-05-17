using BuildingCompany.Application.DTOs;

namespace BuildingCompany.Application.Interfaces;

public interface IProjectTaskService
{
    Task<ProjectTaskDto> CreateTask(ProjectTaskDto projectTaskDto);
    Task<ProjectTaskDto?> GetTask(int id);
    Task<IEnumerable<ProjectTaskDto>> GetTasksByProject(int projectId);
    Task<bool> UpdateTask(ProjectTaskDto projectTaskDto);
    Task<bool> DeleteTask(int id);
    Task<bool> AssignEmployeeToTask(int taskId,int employeeId);
}