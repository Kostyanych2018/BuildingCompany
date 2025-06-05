using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using MongoDB.Bson;

namespace BuildingCompany.Application.Services;

public class TaskMaterialRequirementService(IUnitOfWork unitOfWork) : ITaskMaterialRequirementService
{
    public async Task<TaskMaterialRequirementDto> AddRequirementToTask(TaskMaterialRequirementDto requirementDto)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(requirementDto.TaskId);
        if (task == null)
            throw new ArgumentException("Задача не найдена", nameof(requirementDto.TaskId));
        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirementDto.MaterialId);
        if (material == null)
            throw new ArgumentException("Материал не найден", nameof(requirementDto.MaterialId));

        var requirement = new TaskMaterialRequirement(
            requirementDto.TaskId,
            requirementDto.MaterialId,
            requirementDto.RequiredQuantity
        );
        await unitOfWork.TaskMaterialRequirementRepository.AddAsync(requirement);
        await unitOfWork.SaveAllAsync();
        var result = requirement.ToDto();
        result.Material = material.ToDto();
        return result;
    }

    public async Task<IEnumerable<TaskMaterialRequirementDto>> GetRequirementsDtosByTaskId(ObjectId taskId)
    {
        var requirements = await unitOfWork.TaskMaterialRequirementRepository.GetAllAsync(r=>r.TaskId == taskId);
        var requirementDtos = requirements.ToDto().ToList();
        foreach (var requirementDto in requirementDtos) {
            var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirementDto.MaterialId);
            if (material != null) {
                requirementDto.Material = material.ToDto();
            }
        }

        return requirementDtos;
    }

    public async Task<IEnumerable<TaskMaterialRequirement>> GetRequirementsByTaskId(ObjectId taskId)
    {
        var requirements = await unitOfWork.TaskMaterialRequirementRepository.GetAllAsync(r=>r.TaskId == taskId);
        return requirements.ToList();
    }

    public async Task<bool> UpdateRequirement(TaskMaterialRequirementDto requirementDto)
    {
        var requirement = await unitOfWork.TaskMaterialRequirementRepository.GetByIdAsync(requirementDto.Id);
        if (requirement == null) return false;
        requirement.UpdateRequiredQuantity(requirementDto.RequiredQuantity);

        await unitOfWork.TaskMaterialRequirementRepository.UpdateAsync(requirement);
        await unitOfWork.SaveAllAsync();

        return true;
    }

    public async Task<bool> DeleteRequirement(ObjectId id)
    {
        var requirement = await unitOfWork.TaskMaterialRequirementRepository.GetByIdAsync(id);
        if (requirement == null) return false;

        await unitOfWork.TaskMaterialRequirementRepository.DeleteAsync(requirement);
        await unitOfWork.SaveAllAsync();

        return true;
    }

    public async Task<bool> MarkRequirementAsFulfilled(ObjectId id)
    {
        var requirement = await unitOfWork.TaskMaterialRequirementRepository.GetByIdAsync(id);
        if (requirement == null) return false;

        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirement.MaterialId);
        if (material == null || material.Quantity < requirement.RequiredQuantity)
            return false;

        material.Quantity -= requirement.RequiredQuantity;

        await unitOfWork.MaterialsRepository.UpdateAsync(material);
        await unitOfWork.TaskMaterialRequirementRepository.UpdateAsync(requirement);
        await unitOfWork.SaveAllAsync();

        return true;
    }

    public async Task<decimal> CalculateTotalCostForTask(ObjectId taskId)
    {
        var requirements = await GetRequirementsDtosByTaskId(taskId);
        decimal totalCost = 0;

        foreach (var requirement in requirements) {
            var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirement.MaterialId);
            if (material != null) {
                totalCost += material.FinalPrice * requirement.RequiredQuantity;
            }
        }

        return totalCost;
    }

    public async Task<bool> CheckMaterialAvailability(ObjectId taskId)
    {
        var requirements = await GetRequirementsDtosByTaskId(taskId);

        foreach (var requirement in requirements) {
            var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirement.MaterialId);
            if (material == null || material.Quantity < requirement.RequiredQuantity)
                return false;
        }

        return true;
    }

    public async Task<bool> CompleteTask(ObjectId taskId)
    {
        if (!await CheckMaterialAvailability(taskId))
            return false;

        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null)
            return false;

        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(task.ProjectId);
        if (project == null)
            return false;

        var requirements = await GetRequirementsByTaskId(taskId);
        decimal totalCost = 0;
        foreach (var requirement in requirements) {
            var material = await unitOfWork.MaterialsRepository.GetByIdAsync(requirement.MaterialId);
            if (material == null)
                return false;

            totalCost += material.FinalPrice * requirement.RequiredQuantity;

            material.Quantity -= requirement.RequiredQuantity;
            await unitOfWork.MaterialsRepository.UpdateAsync(material);

            await unitOfWork.TaskMaterialRequirementRepository.UpdateAsync(requirement);
        }

        if (project.Budget < totalCost)
            return false;
        
        project.Budget -= totalCost;
        await unitOfWork.ProjectsRepository.UpdateAsync(project);
        
        task.UpdateStatus(ProjectTaskStatus.Completed);
        task.UpdateCompletionPercentage(100);
        await unitOfWork.ProjectTaskRepository.UpdateAsync(task);

        if (task.AssignedEmployeeId.HasValue) {
            var employee = await unitOfWork.EmployeesRepository.GetByIdAsync(task.AssignedEmployeeId.Value);
            if (employee != null) {
                employee.SetStatus(EmployeeStatus.Available);
                task.AssignedEmployeeId = null;
                employee.AssignedTaskId = null;
                await unitOfWork.EmployeesRepository.UpdateAsync(employee);
            }
        }

        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> CheckBudgetAvailability(ObjectId taskId)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null) 
            return false;
        
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(task.ProjectId);
        if (project == null)
            return false;
        
        decimal totalCost = await CalculateTotalCostForTask(taskId);
    
        return project.Budget >= totalCost;
    }
    public async Task<decimal> GetProjectBudget(ObjectId taskId)
    {
        var task = await unitOfWork.ProjectTaskRepository.GetByIdAsync(taskId);
        if (task == null) 
            return 0;
        
        var project = await unitOfWork.ProjectsRepository.GetByIdAsync(task.ProjectId);
        return project?.Budget ?? 0;
    }
}