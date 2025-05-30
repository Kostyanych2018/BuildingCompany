using System.Globalization;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.ProjectViewModels;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class UpdateProjectViewModel(IProjectService projectService) : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateProjectCommand))]
    private ObjectId _projectId;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateProjectCommand))]
    private string _projectName = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateProjectCommand))]
    private string? _projectDescription;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateProjectCommand))]
    private string _projectBudget = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateProjectCommand))]
    private ProjectDto? _project;


    [RelayCommand]
    private async Task LoadProject()
    {
        var originalProject = await projectService.GetProject(ProjectId);
        if (originalProject == null) {
            await Shell.Current.DisplayAlert("Ошибка", "Проект не найден", "OK");
            return;
        }

        Project = originalProject;
        ProjectName = originalProject.Name;
        ProjectDescription = originalProject.Description;
        ProjectBudget = originalProject.Budget.ToString(CultureInfo.InvariantCulture);
    }

    [RelayCommand(CanExecute = nameof(CanUpdateProject))]
    private async Task UpdateProject()
    {
        decimal.TryParse(ProjectBudget, out var parsedBudget);

        Project.Budget = parsedBudget;
        Project.Name = ProjectName;
        Project.Description = ProjectDescription;

        bool success = await projectService.UpdateProject(Project!);
        if (success) {
            await Shell.Current.DisplayAlert("Успех", "Проект успешно обновлен.", "OK");
        }
        else {
            await Shell.Current.DisplayAlert("Ошибка", "Ошибка при обновлении проекта", "O");
        }
    }

    private bool CanUpdateProject()
    {
        if (Project == null) return false;
        if (string.IsNullOrWhiteSpace(ProjectName)) {
            return false;
        }

        if (!decimal.TryParse(ProjectBudget, out var parsedBudget) || parsedBudget < 0) {
            return false;
        }

        bool nameChanged = !string.Equals(ProjectName, Project?.Name);
        bool desriptionChanged = !string.Equals(ProjectDescription, Project?.Description);
        bool budgetChanged = !string.Equals(ProjectBudget, Project?.Budget.ToString(CultureInfo.InvariantCulture));
        bool hasChanged = nameChanged || desriptionChanged || budgetChanged;

        return hasChanged;
    }
}