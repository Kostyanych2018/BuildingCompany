using System.Collections.Generic;
using System.Linq;
using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Strategies;

public class CompositeQualificationStrategy(IEnumerable<IQualificationStrategy> strategies) : IQualificationStrategy
{
    public bool IsEmployeeQualified(Employee employee, ProjectTask task)
    {
        return strategies.All(strategy => strategy.IsEmployeeQualified(employee, task));
    }
} 