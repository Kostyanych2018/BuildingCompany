using System.Globalization;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;

namespace BuildingCompany.Application.Services;

public class ProjectTaskService(
    IRepository<ProjectTask> projectTaskRepo,
    IRepository<Project> projectRepo,
    IRepository<Employee> employeeRepo) : IProjectTaskService
{
    public async Task<ProjectTaskDto> CreateTask(ProjectTaskDto projectTaskDto)
    {
        var project = await projectRepo.GetByIdAsync(projectTaskDto.ProjectId);
        if (project == null) {
            throw new KeyNotFoundException($"Проект с ID {projectTaskDto.ProjectId} не найден.");
        }

        var task = new ProjectTask(projectTaskDto.Name,
            projectTaskDto.Description,
            projectTaskDto.ProjectId,
            projectTaskDto.CompletionPercentage
        );
        await projectTaskRepo.AddAsync(task);
        return task.ToDto();
    }

    public async Task<ProjectTaskDto?> GetTask(int id)
    {
        var task = await projectTaskRepo.GetByIdAsync(id);
        return task?.ToDto();
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetTasksByProject(int projectId)
    {
        var allTasks = await projectTaskRepo.GetAllAsync();
        var tasks = allTasks.Where(t => t.ProjectId == projectId);
        return tasks.ToDto();
    }

    public async Task<bool> UpdateTask(ProjectTaskDto projectTaskDto)
    {
        var task = await projectTaskRepo.GetByIdAsync(projectTaskDto.Id);
        if (task == null) return false;
        task.UpdateDetails(projectTaskDto.Name, projectTaskDto.Description);
        task.UpdateCompletionPercentage(projectTaskDto.CompletionPercentage);
        if (Enum.TryParse(projectTaskDto.Status, out ProjectTaskStatus status)) {
            task.SetStatus(status);
        }

        await projectTaskRepo.UpdateAsync(task);
        return true;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var task = await projectTaskRepo.GetByIdAsync(id);
        if (task == null) return false;

        await projectTaskRepo.DeleteAsync(task);
        return true;
    }

    public async Task<bool> AssignEmployeeToTask(int taskId, int employeeId)
    {
        var task = await projectTaskRepo.GetByIdAsync(taskId);
        if (task == null) return false;
        var employee = await employeeRepo.GetByIdAsync(employeeId);
        if (employee is not { Status: EmployeeStatus.Available }) return false;

        task.AssignEmployee(employee);
        employee.AssignTask(task);
        employee.SetStatus(EmployeeStatus.Busy);
        await projectTaskRepo.UpdateAsync(task);
        await employeeRepo.UpdateAsync(employee);
        return true;
    }
}