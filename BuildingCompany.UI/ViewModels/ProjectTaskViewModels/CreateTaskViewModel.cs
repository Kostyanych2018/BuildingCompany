using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.ProjectTaskViewModels;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class CreateTaskViewModel : ObservableObject
{
    private readonly IProjectTaskService _projectTaskService;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _requiredPosition = string.Empty;
    [ObservableProperty] private string _requiredExperience = string.Empty;
    [ObservableProperty] private int _requiredCertificationLevel = 1;
    [ObservableProperty] private bool _isRequiredExperienceEnabled = false;
    [ObservableProperty] private bool _isRequiredCertificationEnabled = false;
    [ObservableProperty] private ObjectId _projectId;

    public CreateTaskViewModel(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [RelayCommand]
    private void SetRequiredCertificationLevel(string level)
    {
        if (int.TryParse(level, out int certLevel) && certLevel >= 1 && certLevel <= 5)
        {
            RequiredCertificationLevel = certLevel;
        }
    }

    [RelayCommand]
    private async Task CreateTask()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Название задачи не может быть пустым", "OK");
            return;
        }

        int? requiredExperienceValue = null;
        if (IsRequiredExperienceEnabled && !string.IsNullOrWhiteSpace(RequiredExperience) && 
            int.TryParse(RequiredExperience, out int expValue))
        {
            requiredExperienceValue = expValue;
        }

        int? requiredCertificationValue = null;
        if (IsRequiredCertificationEnabled)
        {
            requiredCertificationValue = RequiredCertificationLevel;
        }

        var taskDto = new ProjectTaskDto
        {
            Name = Name,
            Description = Description,
            ProjectId = ProjectId,
            CompletionPercentage = 0,
            Status = "Created",
            RequiredPosition = string.IsNullOrWhiteSpace(RequiredPosition) ? null : RequiredPosition,
            RequiredExperience = requiredExperienceValue,
            RequiredCertificationLevel = requiredCertificationValue
        };

        try
        {
            await _projectTaskService.CreateTask(taskDto);
            await Shell.Current.DisplayAlert("Успех", "Задача создана", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось создать задачу: {ex.Message}", "OK");
        }
    }
} 