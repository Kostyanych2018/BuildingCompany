using BuildingCompany.Application.DTOs;
using BuildingCompany.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingCompany.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IRepository<Project>, InMemoryProjectRepository>()
            .AddSingleton<IRepository<ProjectTask>, InMemoryProjectTaskRepository>()
            .AddSingleton<IRepository<Employee>, InMemoryEmployeeRepository>();
        return services;
    }
}