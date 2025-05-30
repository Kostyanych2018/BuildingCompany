using System;
using System.Globalization;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Domain.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.ProjectViewModels;

public partial class CreateProjectViewModel(IProjectService projectService) : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateProjectCommand))]
    private string _projectName = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateProjectCommand))]
    private string? _projectDescription;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateProjectCommand))]
    private string _projectBudget="";
    

    [RelayCommand(CanExecute = nameof(CanCreateProject))]
    private async Task CreateProject()
    {
        decimal.TryParse(ProjectBudget, out var budget);
        var dto = new ProjectDto()
        {
            Name = ProjectName,
            Budget = budget,
            Description = ProjectDescription
        };
        var createdProject = await projectService.CreateProject(dto);
        await Shell.Current.DisplayAlert("Успех", $"Проект '{createdProject.Name}' успешно создан.", "OK");
        ProjectDescription = null;
        ProjectName = "";
        ProjectBudget = "";
    }

    private bool CanCreateProject()
    {
        return !string.IsNullOrWhiteSpace(ProjectName)
               && decimal.TryParse(ProjectBudget, out var budget)
               && budget > 0;
    }
}