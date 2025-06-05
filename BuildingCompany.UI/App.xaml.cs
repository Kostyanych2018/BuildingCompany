using BuildingCompany.UI.Pages.EmployeePages;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
        
        // Инициализируем главную страницу один раз
        MainPage = new AppShell();
    }    

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Используем уже созданный MainPage вместо создания нового AppShell
        var window = new Window(MainPage)
        {
            Height = 600,
            Width = 500
        };
        return window;
    }
}