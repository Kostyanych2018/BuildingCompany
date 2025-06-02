using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Strategies;

public interface IQualificationStrategy
{
    bool IsEmployeeQualified(Employee employee, ProjectTask task);
} 