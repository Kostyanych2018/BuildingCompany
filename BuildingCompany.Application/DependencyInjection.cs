using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingCompany.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddTransient<IProjectService, ProjectService>()
            .AddTransient<IProjectTaskService, ProjectTaskService>()
            .AddTransient<IEmployeeService, EmployeeService>();
        return services;
    }
}