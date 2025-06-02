using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface ITaskMaterialRequirementService
{
    Task<TaskMaterialRequirementDto> AddRequirementToTask(TaskMaterialRequirementDto requirementDto);
    Task<IEnumerable<TaskMaterialRequirementDto>> GetRequirementsDtosByTaskId(ObjectId taskId);
    Task<IEnumerable<TaskMaterialRequirement>> GetRequirementsByTaskId(ObjectId taskId);
    Task<bool> UpdateRequirement(TaskMaterialRequirementDto requirementDto);
    Task<bool> DeleteRequirement(ObjectId id);
    Task<bool> MarkRequirementAsFulfilled(ObjectId id);
    Task<decimal> CalculateTotalCostForTask(ObjectId taskId);
    Task<bool> CheckMaterialAvailability(ObjectId taskId);
    Task<bool> CompleteTask(ObjectId taskId);
    Task<bool> CheckBudgetAvailability(ObjectId taskId);
    Task<decimal> GetProjectBudget(ObjectId taskId);
}   