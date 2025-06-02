using System.Collections.ObjectModel;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.ProjectTaskViewModels;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class ProjectTaskDetailsViewModel(
    IEmployeeService employeeService,
    IProjectTaskService projectTaskService,
    IMaterialService materialService,
    ITaskMaterialRequirementService taskMaterialRequirementService) : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))]
    private ProjectTaskDto? _selectedTask;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))]
    private EmployeeDto? _assignedEmployee;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))]
    private EmployeeDto? _selectedEmployee;

    [ObservableProperty] private ObjectId _taskId;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddMaterialRequirementCommand))]
    private MaterialDto? _selectedMaterial;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddMaterialRequirementCommand))]
    private int _requiredQuantity;

    [ObservableProperty] private TaskMaterialRequirementDto? _selectedRequirement;

    [ObservableProperty] private decimal _totalCost;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CompleteTaskCommand))]
    private bool _allMaterialsAvailable;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CompleteTaskCommand))]
    private bool _isBudgetAvailable;

    [ObservableProperty] private decimal _projectBudget;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private int _animatedProgressValue;
    [ObservableProperty] private double _animatedProgressPercent;

    public bool IsTaskNotCompleted => SelectedTask?.Status != "Completed";

    public ObservableCollection<EmployeeDto> Employees { get; } = [];
    public ObservableCollection<MaterialDto> Materials { get; } = [];
    public ObservableCollection<TaskMaterialRequirementDto> MaterialRequirements { get; } = [];

    [RelayCommand]
    private async Task LoadTask()
    {
        SelectedTask = await projectTaskService.GetTask(TaskId);
        if (SelectedTask?.AssignedEmployeeId != null) {
            AssignedEmployee = await employeeService.GetEmployee(SelectedTask.AssignedEmployeeId.Value);
        }

        AnimatedProgressValue = SelectedTask?.CompletionPercentage ?? 0;
        AnimatedProgressPercent = AnimatedProgressValue / 100.0;
        OnPropertyChanged(nameof(IsTaskNotCompleted));

        await LoadMaterialRequirements();
    }

    [RelayCommand]
    private async Task LoadMaterials()
    {
        var materials = await materialService.GetMaterials();
        if (Materials.Count > 0) {
            Materials.Clear();
        }

        foreach (var material in materials) {
            Materials.Add(material);
        }
    }

    [RelayCommand]
    private async Task LoadMaterialRequirements()
    {
        if (SelectedTask == null) return;

        var requirements = await taskMaterialRequirementService.GetRequirementsDtosByTaskId(SelectedTask.Id);
        MaterialRequirements.Clear();

        foreach (var requirement in requirements) {
            MaterialRequirements.Add(requirement);
        }

        TotalCost = await taskMaterialRequirementService.CalculateTotalCostForTask(SelectedTask.Id);
        AllMaterialsAvailable = await taskMaterialRequirementService.CheckMaterialAvailability(SelectedTask.Id);
        ProjectBudget = await taskMaterialRequirementService.GetProjectBudget(SelectedTask.Id);
        IsBudgetAvailable = await taskMaterialRequirementService.CheckBudgetAvailability(SelectedTask.Id);
        CompleteTaskCommand.NotifyCanExecuteChanged();
        UpdateStatusMessage();
    }

    [RelayCommand]
    private async Task LoadEmployees()
    {
        if (SelectedTask == null) return;

        var employees = await projectTaskService.GetQualifiedEmployees(SelectedTask.Id);

        if (Employees.Count > 0) {
            Employees.Clear();
        }

        foreach (var employee in employees) {
            Employees.Add(employee);
        }

        if (!Employees.Any() && AssignedEmployee == null) {
            await Shell.Current.DisplayAlert("Внимание",
                $"Нет доступных сотрудников с требуемой квалификацией: {SelectedTask.RequiredPosition ?? "не указана"}",
                "OK");
        }
    }

    [RelayCommand(CanExecute = nameof(CanAddMaterialRequirement))]
    private async Task AddMaterialRequirement()
    {
        var requirement = new TaskMaterialRequirementDto
        {
            TaskId = SelectedTask!.Id,
            MaterialId = SelectedMaterial!.Id,
            RequiredQuantity = RequiredQuantity,
            Material = SelectedMaterial
        };
        try {
            var result = await taskMaterialRequirementService.AddRequirementToTask(requirement);
            MaterialRequirements.Add(result);

            SelectedMaterial = null;
            RequiredQuantity = 0;

            TotalCost = await taskMaterialRequirementService.CalculateTotalCostForTask(SelectedTask.Id);
            AllMaterialsAvailable = await taskMaterialRequirementService.CheckMaterialAvailability(SelectedTask.Id);
            CompleteTaskCommand.NotifyCanExecuteChanged();
            UpdateStatusMessage();
        }
        catch (Exception ex) {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось добавить требование: {ex.Message}", "OK");
        }
    }

    private bool CanAddMaterialRequirement()
    {
        return SelectedTask != null &&
               SelectedMaterial != null &&
               RequiredQuantity > 0;
    }

    [RelayCommand]
    private async Task DeleteRequirement(TaskMaterialRequirementDto requirement)
    {
        if (requirement == null) return;
        bool confirm = await Shell.Current.DisplayAlert(
            "Подтверждение",
            $"Вы уверены, что хотите удалить требование '{requirement.Material?.Name}'?",
            "Да", "Отмена");
        if (!confirm) return;
        bool success = await taskMaterialRequirementService.DeleteRequirement(requirement.Id);
        if (success) {
            MaterialRequirements.Remove(requirement);
            TotalCost = await taskMaterialRequirementService.CalculateTotalCostForTask(SelectedTask!.Id);
            AllMaterialsAvailable = await taskMaterialRequirementService.CheckMaterialAvailability(SelectedTask.Id);
        }
        else {
            await Shell.Current.DisplayAlert("Ошибка", "Не удалось удалить требование", "OK");
        }

        CompleteTaskCommand.NotifyCanExecuteChanged();
        UpdateStatusMessage();
    }

    [RelayCommand(CanExecute = nameof(CanAssignEmployee))]
    private async Task AssignEmployee()
    {
        bool success = await projectTaskService.AssignEmployeeToTask(SelectedTask!.Id, SelectedEmployee!.Id);
        if (success) {
            await LoadTask();
        }
        else {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось назначить сотрудника '{SelectedEmployee.FullName}'," +
                                                       $" возможно он занят другой задачей.", "OK");
        }
    }

    private bool CanAssignEmployee()
    {
        return SelectedEmployee != null
               && SelectedTask?.AssignedEmployeeId == null;
    }

    private void UpdateStatusMessage()
    {
        if (MaterialRequirements.Count == 0) {
            StatusMessage = "Нет требований к материалам";
        }
        else if (!AllMaterialsAvailable) {
            StatusMessage = "Недостаточно материалов";
        }
        else if (!IsBudgetAvailable) {
            StatusMessage = "Недостаточно бюджета";
        }
        else if (SelectedTask?.AssignedEmployeeId == null) {
            StatusMessage = "Не назначен сотрудник";
        }
        else if (!AllMaterialsAvailable && !IsBudgetAvailable) {
            StatusMessage = "Недостаточно материалов и бюджета";
        }
        else {
            StatusMessage = "Все требования выполнены";
        }
    }

    [RelayCommand(CanExecute = nameof(CanCompleteTask))]
    private async Task CompleteTask()
    {
        int startValue = SelectedTask!.CompletionPercentage;
        int endValue = 100;
        int step = 5;

        for (int i = startValue; i <= endValue; i += step) {
            AnimatedProgressValue = i;
            AnimatedProgressPercent = i / 100.0;
            await Task.Delay(100);
        }

        bool success = await taskMaterialRequirementService.CompleteTask(SelectedTask.Id);
        if (success) {
            await LoadTask();
            await Shell.Current.DisplayAlert("Успех", "Задача выполнена! Материалы использованы, сотрудник освобожден.", "OK");
        }
        else {
            AnimatedProgressValue = startValue;
            AnimatedProgressPercent = startValue / 100.0;
            await Shell.Current.DisplayAlert("Ошибка", "Не удалось выполнить задачу. Проверьте наличие материалов.", "OK");
        }
    }

    private bool CanCompleteTask()
    {
        return SelectedTask != null &&
               SelectedTask.AssignedEmployeeId != null &&
               SelectedTask.Status != "Completed" &&
               MaterialRequirements.Count > 0 &&
               AllMaterialsAvailable && IsBudgetAvailable;
    }
}