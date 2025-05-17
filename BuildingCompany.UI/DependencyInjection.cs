using BuildingCompany.UI.Pages.EmployeePages;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using BuildingCompany.UI.ViewModels.EmployeeViewModels;
using BuildingCompany.UI.ViewModels.ProjectTaskViewModels;
using BuildingCompany.UI.ViewModels.ProjectViewModels;
using ProjectsPage = BuildingCompany.UI.Pages.ProjectPages.ProjectsPage;
using ProjectsViewModel = BuildingCompany.UI.ViewModels.ProjectViewModels.ProjectsViewModel;

namespace BuildingCompany.UI;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services
            .AddTransient<ProjectsPage>()
            .AddTransient<CreateProjectPage>()
            .AddTransient<ProjectTaskDetailsPage>()
            .AddTransient<EmployeesPage>();
        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services
            .AddTransient<ProjectsViewModel>()
            .AddTransient<CreateProjectViewModel>()
            .AddTransient<ProjectTaskDetailsViewModel>()
            .AddTransient<EmployeesViewModel>();
        return services;
    }
}