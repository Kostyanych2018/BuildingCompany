using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<Project> ProjectsRepository { get; }
    IRepository<ProjectTask> ProjectTaskRepository { get; }
    IRepository<Employee> EmployeesRepository { get; }
    IRepository<Material> MaterialsRepository { get; }
    
    Task SaveAllAsync();
    Task CreateDatabaseAsync();
    Task DeleteDatabaseAsync();
}