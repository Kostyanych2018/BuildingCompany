using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.ViewModels.ProjectViewModels;

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
    private async Task GoToCreateProject() => await Shell.Current.GoToAsync("CreateProjectPage");

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
    private async Task LoadProjects()
    {
        Message = "";
        var projectDtos = (await _projectService.GetProjects()).ToList();
        Projects.Clear();
        foreach (var dto in projectDtos) {
            Projects.Add(dto);
        }

        if (projectDtos.Count == 0) {
            Message = "Проектов пока нет.";
        }
    }

    [RelayCommand]
    private async Task LoadProjectTasks(int projectId)
    {
        Message = "";
        var tasks = (await _projectTaskService.GetTasksByProject(projectId)).ToList();
        Tasks.Clear();
        foreach (var task in tasks) {
            Tasks.Add(task);
        }

        if (tasks.Count == 0) {
            Message = "Задач у выбранного проекта пока нет.";
        }
    }

    [RelayCommand]
    private async Task DeleteProject()
    {
        if (SelectedProject == null) {
            Message = "Сначала выберите проект для удаления.";
            return;
        }

        bool confirmed = await Shell.Current.DisplayAlert("Удаление",
            $"Удалить проект '{SelectedProject.Name}'?", "Да", "Нет");
        if (!confirmed) return;
        bool succes = await _projectService.DeleteProject(SelectedProject.Id);
        if (succes) {
            Message = $"Проект '{SelectedProject.Name}' удален.";
            Projects.Remove(SelectedProject);
        }
        else {
            Message = "Не удалось удалить проект.";
        }
    }
}