using BuildingCompany.Infrastructure.Data;
using BuildingCompany.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingCompany.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,DbContextOptions<AppDbContext> options)
    {
        services.AddSingleton<IUnitOfWork, EfUnitOfWork>()
            .AddSingleton(new AppDbContext(options));
        return services;
    }
}