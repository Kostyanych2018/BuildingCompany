using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Services;
using BuildingCompany.Domain.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BuildingCompany.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddTransient<IProjectService, ProjectService>()
            .AddTransient<IProjectTaskService, ProjectTaskService>()
            .AddTransient<IEmployeeService, EmployeeService>()
            .AddTransient<IMaterialService, MaterialService>()
            .AddTransient<ITaskMaterialRequirementService, TaskMaterialRequirementService>();

        services.AddSingleton<PositionQualificationStrategy>();
        services.AddSingleton<ExperienceQualificationStrategy>();
        services.AddSingleton<CertificationQualificationStrategy>();
        
        services.AddSingleton<IQualificationStrategy>(provider => 
        {
            var strategies = new List<IQualificationStrategy>
            {
                provider.GetRequiredService<PositionQualificationStrategy>(),
                provider.GetRequiredService<ExperienceQualificationStrategy>(),
                provider.GetRequiredService<CertificationQualificationStrategy>()
            };
            
            return new CompositeQualificationStrategy(strategies);
        });
        
        return services;
    }
}