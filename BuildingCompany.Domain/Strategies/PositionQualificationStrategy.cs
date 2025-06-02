using System;
using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Strategies;

/// <summary>
/// Стратегия проверки квалификации сотрудника по должности
/// </summary>
public class PositionQualificationStrategy : IQualificationStrategy
{
    public bool IsEmployeeQualified(Employee employee, ProjectTask task)
    {
        if (string.IsNullOrEmpty(task.RequiredPosition))
            return true;
            
        return string.Equals(employee.Position, task.RequiredPosition, StringComparison.OrdinalIgnoreCase);
    }
}