using System;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildingCompany.UI.ViewModels.ProjectViewModels;

public partial class CreateProjectViewModel(IProjectService projectService) : ObservableObject
{

    [ObservableProperty] private string _projectName = "";
    [ObservableProperty] private string? _projectDescription;
    [ObservableProperty] private string _projectBudget;
    [ObservableProperty] private string _message = "";

    [RelayCommand]
    private async Task CreateProject()
    {
        if (string.IsNullOrWhiteSpace(ProjectName)) {
            Message = "Название проекта не может быть пустым.";
            return;
        }
        if (!decimal.TryParse(ProjectBudget,out var budget) || budget < 0) {
            Message = "Бюджет не может быть отрицательным или введен неверно.";
            return;
        }

        var dto = new ProjectDto()
        {
            Name = ProjectName,
            Budget = budget,
            Description = ProjectDescription
        };
        try {
            var createdProject = await projectService.CreateProject(dto);
            Message = $"Проект '{createdProject.Name}' успешно создан.";
            ProjectName = "";
            ProjectDescription = null;
            ProjectBudget = "";
        }
        catch (Exception ex) {
            Message = $"Произошла ошибка: {ex.Message}";
        }
    }
}