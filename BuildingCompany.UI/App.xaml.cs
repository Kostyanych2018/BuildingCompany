using BuildingCompany.UI.Pages.EmployeePages;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
    }
            

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window =  new Window(new AppShell())
        {
            Height = 600,
            Width = 500
        };
        return window;
    }
}