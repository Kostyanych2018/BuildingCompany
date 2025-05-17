using System;
using Microsoft.Maui.Controls;
using ProjectsViewModel = BuildingCompany.UI.ViewModels.ProjectViewModels.ProjectsViewModel;

namespace BuildingCompany.UI.Pages.ProjectPages;

public partial class ProjectsPage : ContentPage
{
    private readonly ProjectsViewModel _viewModel;

    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadProjectsCommand.CanExecute(null)) {
            await _viewModel.LoadProjectsCommand.ExecuteAsync(null);
        }
    }

    private async void LoadProjectTasksHandler(object? sender, EventArgs e)
    {
        if (_viewModel.SelectedProject != null) {
            await _viewModel.LoadProjectTasksCommand.ExecuteAsync(_viewModel.SelectedProject.Id);
        }
    }
}