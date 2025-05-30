using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface IProjectTaskService
{
    Task<ProjectTaskDto> CreateTask(ProjectTaskDto projectTaskDto);
    Task<ProjectTaskDto?> GetTask(ObjectId id);
    Task<IEnumerable<ProjectTaskDto>> GetTasksByProject(ObjectId projectId);
    Task<bool> UpdateTask(ProjectTaskDto projectTaskDto);
    Task<bool> DeleteTask(ObjectId id);
    Task<bool> AssignEmployeeToTask(ObjectId taskId, ObjectId employeeId);
}