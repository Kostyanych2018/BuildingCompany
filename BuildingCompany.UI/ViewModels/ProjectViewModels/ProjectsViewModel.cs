using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.ProjectPages;
using BuildingCompany.UI.Pages.ProjectTaskPages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

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

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CompleteProjectCommand))]
    private bool _canCompleteSelectedProject;

    [ObservableProperty] private PlotModel _projectStatusChart;

    public ProjectsViewModel(IProjectService projectService, IProjectTaskService projectTaskService)
    {
        _projectService = projectService;
        _projectTaskService = projectTaskService;
        
        // Initialize empty chart
        ProjectStatusChart = new PlotModel { 
            Title = "Статус проектов",
            TitleColor = OxyColors.DarkBlue
        };
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
            { "projectId", SelectedProject!.Id }
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
        
        UpdateProjectStatusChart(projectDtos);
    }
    
    private void UpdateProjectStatusChart(List<ProjectDto> projects)
    {
        var model = new PlotModel { 
            Title = "Статус проектов",
            TitleColor = OxyColors.Chocolate
        };

        
        var categoryAxis = new CategoryAxis { 
            Position = AxisPosition.Left,
            Title = "Статус",
            TitleColor = OxyColors.Black,
            TextColor = OxyColors.Black
        };
        
        var valueAxis = new LinearAxis {
            Position = AxisPosition.Bottom,
            Title = "Количество проектов",
            TitleColor = OxyColors.Black,
            TextColor = OxyColors.Black,
            MinimumPadding = 0,
            MaximumPadding = 0.1,
            AbsoluteMinimum = 0
        };
        
        var statusCounts = projects
            .GroupBy(p => p.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToList();
        
        foreach (var status in statusCounts)
        {
            categoryAxis.Labels.Add(status.Status);
        }
        
        var series = new BarSeries
        {
            IsStacked = false,
            StrokeColor = OxyColors.Black,
            StrokeThickness = 1,
        };
        
        for (int i = 0; i < statusCounts.Count; i++)
        {
            series.Items.Add(new BarItem { 
                Value = statusCounts[i].Count,
                Color = GetStatusColor(statusCounts[i].Status)
            });
        }
        
        model.Axes.Add(categoryAxis);
        model.Axes.Add(valueAxis);
        model.Series.Add(series);
        
        ProjectStatusChart = model;
    }
    
    private OxyColor GetStatusColor(string status)
    {
        return status switch
        {
            "Created" => OxyColors.Gray,
            "InProgress" => OxyColors.Orange,
            "Completed" => OxyColors.Green,
            _ => OxyColors.Blue
        };
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

        CanCompleteSelectedProject = SelectedProject != null &&
                                     SelectedProject.Status != "Completed" &&
                                     tasks.Count > 0 &&
                                     tasks.All(t => t.CompletionPercentage == 100);
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
            await Shell.Current.DisplayAlert("Удаление", $"Проект '{SelectedProject.Name}' удален.", "OK");
            Projects.Remove(SelectedProject);
            
            // Update chart after deletion
            UpdateProjectStatusChart(Projects.ToList());
        }
        else {
            await Shell.Current.DisplayAlert("Удаление", "Не удалось удалить проект.", "OK");
        }
    }

    [RelayCommand(CanExecute = nameof(CanCompleteProject))]
    private async Task CompleteProject()
    {
        if (SelectedProject == null) return;

        bool confirmed = await Shell.Current.DisplayAlert("Завершение проекта",
            $"Завершить проект '{SelectedProject.Name}'?\n\nЭто действие:\n- Изменит статус проекта на 'Завершен'\n- Освободит всех сотрудников\n- Удалит все материальные требования",
            "Завершить", "Отмена");

        if (!confirmed) return;

        try
        {
            bool success = await _projectService.CompleteProject(SelectedProject.Id);

            if (success)
            {
                SelectedProject.Status = "Completed";

                await LoadProjects();
                await LoadProjectTasks(SelectedProject.Id);

                await Shell.Current.DisplayAlert("Успех",
                    $"Проект '{SelectedProject.Name}' успешно завершен. Все ресурсы освобождены.",
                    "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    "Не удалось завершить проект. Убедитесь, что все задачи выполнены на 100%.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка",
                $"Произошла ошибка при завершении проекта: {ex.Message}",
                "OK");
        }
    }

    private bool CanCompleteProject()
    {
        return CanCompleteSelectedProject;
    }
}