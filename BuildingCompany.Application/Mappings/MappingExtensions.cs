using BuildingCompany.Application.DTOs;

namespace BuildingCompany.Application.Mappings;

public static class MappingExtensions
{
    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Budget = project.Budget,
            Status = project.Status.ToString(),
        };
    }

    public static IEnumerable<ProjectDto> ToDto(this IEnumerable<Project> projects)
    {
        return projects.Select(p => p.ToDto());
    }

    public static ProjectTaskDto ToDto(this ProjectTask projectTask)
    {
        return new ProjectTaskDto()
        {
            Id = projectTask.Id,
            Name = projectTask.Name,
            Description = projectTask.Description,
            Status = projectTask.Status.ToString(),
            CompletionPercentage = projectTask.CompletionPercentage,
            ProjectId = projectTask.ProjectId,
            AssignedEmployeeId = projectTask.AssignedEmployeeId
        };
    }

    public static IEnumerable<ProjectTaskDto> ToDto(this IEnumerable<ProjectTask> projectTasks)
    {
        return projectTasks.Select(t => t.ToDto());
    }

    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto()
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Position = employee.Position,
            Status = employee.Status.ToString(),
            AssignedTaskId = employee.AssignedTaskId
        };
    }

    public static IEnumerable<EmployeeDto> ToDto(this IEnumerable<Employee> employees)
    {
        return employees.Select(e => e.ToDto());
    }
}