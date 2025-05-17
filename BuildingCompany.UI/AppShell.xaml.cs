using BuildingCompany.UI.Pages;
using BuildingCompany.UI.Pages.EmployeePages;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
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
    }
}