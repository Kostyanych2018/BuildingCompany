using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.ProjectViewModels;
/// <summary>
/// Модель для управления прокектами
/// Просмотр, удаление, переход на создание
/// </summary>
[SuppressMessage("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "MVVMTK0045:Using [ObservableProperty] on fields is not AOT compatible for WinRT")]
public partial class ProjectsViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly IProjectTaskService _projectTaskService;

    public ObservableCollection<ProjectDto> Projects { get; } = [];
    public ObservableCollection<ProjectTaskDto> Tasks { get; } = [];

    [ObservableProperty] private ProjectDto? _selectedProject;
    [ObservableProperty] private string _message = "";

    public ProjectsViewModel(IProjectService projectService, IProjectTaskService projectTaskService)
    {
        _projectService = projectService;
        _projectTaskService = projectTaskService;
    }

    [RelayCommand]
    private async Task GoToCreateProject() => await Shell.Current.GoToAsync(nameof(CreateProjectPage));

    [RelayCommand]
    private async Task GoToTaskDetails(ProjectTaskDto taskDto)
    {
        IDictionary<string, object> dict = new Dictionary<string, object>()
        {
            { "taskId", taskDto.Id }
        };
        await Shell.Current.GoToAsync(nameof(ProjectTaskDetailsPage), dict);
    }

    [RelayCommand]
    private async Task GoToCreateTask()
    {
        IDictionary<string, object> dict = new Dictionary<string, object>()
        {
            { "projectId",SelectedProject!.Id }
        };
        await Shell.Current.GoToAsync(nameof(CreateTaskPage), dict);
    }

    [RelayCommand]
    private async Task UpdateProject()
    {
        IDictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "projectId", SelectedProject.Id }
        };
        await Shell.Current.GoToAsync(nameof(UpdateProjectPage), parameters);
    }


    [RelayCommand]
    private async Task LoadProjects()
    {
        var projectDtos = (await _projectService.GetProjects()).ToList();
        Projects.Clear();
        foreach (var dto in projectDtos) {
            Projects.Add(dto);
        }

        if (projectDtos.Count == 0) {
            Message = "Проектов пока нет";
        }
    }

    [RelayCommand]
    private async Task LoadProjectTasks(ObjectId projectId)
    {
        Message = "";
        var tasks = (await _projectTaskService.GetTasksByProject(projectId)).ToList();
        Tasks.Clear();
        foreach (var task in tasks) {
            Tasks.Add(task);
        }
    }

    [RelayCommand]
    private async Task DeleteProject()
    {
        if (SelectedProject == null) {
            return;
        }

        bool confirmed = await Shell.Current.DisplayAlert("Удаление",
            $"Удалить проект '{SelectedProject.Name}'?", "Да", "Нет");
        if (!confirmed) return;
        bool succes = await _projectService.DeleteProject(SelectedProject.Id);
        if (succes) {
            await Shell.Current.DisplayAlert("Удаление",$"Проект '{SelectedProject.Name}' удален.","OK");
            Projects.Remove(SelectedProject);
        }
        else {
            await Shell.Current.DisplayAlert("Удаление","Не удалось удалить проект.","OK");
        }
    }
}