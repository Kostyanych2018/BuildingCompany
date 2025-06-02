using System.Collections.Generic;
using System.Linq;
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

    public static ProjectTaskDto ToDto(this ProjectTask task)
    {
        return new ProjectTaskDto
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status.ToString(),
            CompletionPercentage = task.CompletionPercentage,
            ProjectId = task.ProjectId,
            AssignedEmployeeId = task.AssignedEmployeeId,
            RequiredPosition = task.RequiredPosition,
            RequiredExperience = task.RequiredExperience,
            RequiredCertificationLevel = task.RequiredCertificationLevel
        };
    }

    public static IEnumerable<ProjectTaskDto> ToDto(this IEnumerable<ProjectTask> projectTasks)
    {
        return projectTasks.Select(t => t.ToDto());
    }
    public static ProjectTask ToDomain(this ProjectTaskDto dto)
    {
        var task = new ProjectTask
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            CompletionPercentage = dto.CompletionPercentage,
            AssignedEmployeeId = dto.AssignedEmployeeId,
            RequiredPosition = dto.RequiredPosition,
            RequiredExperience = dto.RequiredExperience,
            RequiredCertificationLevel = dto.RequiredCertificationLevel
        };

        if (System.Enum.TryParse<ProjectTaskStatus>(dto.Status, out var status))
        {
            task.Status = status;
        }

        return task;
    }

    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto()
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Position = employee.Position,
            Status = employee.Status.ToString(),
            Experience = employee.Experience,
            CertificationLevel = employee.CertificationLevel,
            AssignedTaskId = employee.AssignedTaskId
        };
    }

    public static IEnumerable<EmployeeDto> ToDto(this IEnumerable<Employee> employees)
    {
        return employees.Select(e => e.ToDto());
    }
    
    public static Employee ToDomain(this EmployeeDto dto)
    {
        var employee = new Employee
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Position = dto.Position,
            Experience = dto.Experience,
            CertificationLevel = dto.CertificationLevel,
            AssignedTaskId = dto.AssignedTaskId
        };

        if (System.Enum.TryParse<EmployeeStatus>(dto.Status, out var status))
        {
            employee.Status = status;
        }

        return employee;
    }

    public static MaterialDto ToDto(this Material material)
    {
        return new MaterialDto()
        {
            Id = material.Id,
            Name = material.Name,
            UnitPrice = material.UnitPrice,
            UnitOfMeasure = material.UnitOfMeasure,
            Quantity = material.Quantity,
        };
    }
    

    public static IEnumerable<MaterialDto> ToDto(this IEnumerable<Material> materials)
    {
        return materials.Select(m => m.ToDto());
    }

    public static TaskMaterialRequirementDto ToDto(this TaskMaterialRequirement requirement)
    {
        return new TaskMaterialRequirementDto
        {
            Id = requirement.Id,
            TaskId = requirement.TaskId,
            MaterialId = requirement.MaterialId,
            RequiredQuantity = requirement.RequiredQuantity,
            IsFulfilled = requirement.IsFulfilled
        };
    }

    public static IEnumerable<TaskMaterialRequirementDto> ToDto(this IEnumerable<TaskMaterialRequirement> requirements)
    {
        return requirements.Select(r => r.ToDto());
    }
}