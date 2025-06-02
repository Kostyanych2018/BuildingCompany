using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Strategies;

public class CertificationQualificationStrategy : IQualificationStrategy
{
    public bool IsEmployeeQualified(Employee employee, ProjectTask task)
    {
        if (!task.RequiredCertificationLevel.HasValue)
            return true;
            
        return employee.CertificationLevel >= task.RequiredCertificationLevel.Value;
    }
} 