using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.ProjectTaskViewModels;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.Pages.ProjectTaskPages;

public partial class ProjectTaskDetailsPage : ContentPage
{
    private readonly ProjectTaskDetailsViewModel _viewModel;
    public ProjectTaskDetailsPage(ProjectTaskDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTaskCommand.ExecuteAsync(null);
        await _viewModel.LoadEmployeesCommand.ExecuteAsync(null);
    }
}