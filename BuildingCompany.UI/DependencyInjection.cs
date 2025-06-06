using BuildingCompany.UI.Pages;
using BuildingCompany.UI.Pages.EmployeePages;
using BuildingCompany.UI.Pages.MaterialPages;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using BuildingCompany.UI.ViewModels.EmployeeViewModels;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;
using BuildingCompany.UI.ViewModels.ProjectTaskViewModels;
using BuildingCompany.UI.ViewModels.ProjectViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
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
            .AddTransient<UpdateProjectPage>()
            .AddTransient<ProjectTaskDetailsPage>()
            .AddTransient<CreateTaskPage>()
            .AddTransient<EmployeesPage>()
            .AddTransient<CreateEmployeePage>()
            .AddTransient<UpdateEmployeePage>()
            .AddTransient<MaterialsPage>()
            .AddTransient<CreateMaterialPage>()
            .AddTransient<MaterialDetailsPage>()
            .AddTransient<UpdateMaterialPage>()
            .AddTransient<HomePage>();
            
        return services;
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services
            .AddTransient<ProjectsViewModel>()
            .AddTransient<CreateProjectViewModel>()
            .AddTransient<UpdateProjectViewModel>()
            .AddTransient<ProjectTaskDetailsViewModel>()
            .AddTransient<CreateTaskViewModel>()
            .AddTransient<EmployeesViewModel>()
            .AddTransient<CreateEmployeeViewModel>()
            .AddTransient<UpdateEmployeeViewModel>()
            .AddTransient<MaterialsViewModel>()
            .AddTransient<CreateMaterialViewModel>()
            .AddTransient<MaterialDetailsViewModel>()
            .AddTransient<UpdateMaterialViewModel>();
        return services;
    }
}