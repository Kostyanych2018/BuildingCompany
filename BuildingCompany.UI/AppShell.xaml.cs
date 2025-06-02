using BuildingCompany.UI.Pages;
using BuildingCompany.UI.Pages.EmployeePages;
using BuildingCompany.UI.Pages.MaterialPages;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;
using Microsoft.Maui.Controls;
using ProjectsPage = BuildingCompany.UI.Pages.ProjectPages.ProjectsPage;

namespace BuildingCompany.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CreateProjectPage),typeof(CreateProjectPage));
        Routing.RegisterRoute(nameof(ProjectTaskDetailsPage),typeof(ProjectTaskDetailsPage));
        Routing.RegisterRoute(nameof(UpdateProjectPage),typeof(UpdateProjectPage));
        Routing.RegisterRoute(nameof(CreateTaskPage),typeof(CreateTaskPage));
        Routing.RegisterRoute(nameof(CreateEmployeePage),typeof(CreateEmployeePage));
        Routing.RegisterRoute(nameof(UpdateEmployeePage),typeof(UpdateEmployeePage));
        Routing.RegisterRoute(nameof(UpdateEmployeePage),typeof(UpdateEmployeePage));
        Routing.RegisterRoute(nameof(CreateMaterialPage),typeof(CreateMaterialPage));
        Routing.RegisterRoute(nameof(MaterialDetailsPage),typeof(MaterialDetailsPage));
    }
}