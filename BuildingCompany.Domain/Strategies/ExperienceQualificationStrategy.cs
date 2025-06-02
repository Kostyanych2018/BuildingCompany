using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Strategies;

public class ExperienceQualificationStrategy : IQualificationStrategy
{
    public bool IsEmployeeQualified(Employee employee, ProjectTask task)
    {
        if (!task.RequiredExperience.HasValue)
            return true;
            
        return employee.Experience >= task.RequiredExperience.Value;
    }
} 