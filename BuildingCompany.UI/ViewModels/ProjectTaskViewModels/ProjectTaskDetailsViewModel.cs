using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.ViewModels.ProjectTaskViewModels;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class ProjectTaskDetailsViewModel(
    IEmployeeService employeeService,
    IProjectTaskService projectTaskService) : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))]
    private ProjectTaskDto _selectedTask;

    [ObservableProperty] private int _taskId;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))] 
    private EmployeeDto? _assignedEmployee;
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AssignEmployeeCommand))]
    private EmployeeDto? _selectedEmployee;

    public ObservableCollection<EmployeeDto> Employees { get; } = [];

    [RelayCommand]
    private async Task LoadTask()
    {
        if (TaskId < 0) return;
        SelectedTask = (await projectTaskService.GetTask(TaskId))!;
        if (SelectedTask.AssignedEmployeeId.HasValue) {
            AssignedEmployee = await employeeService.GetEmployee(SelectedTask.AssignedEmployeeId.Value);
        }
    }

    [RelayCommand]
    private async Task LoadEmployees()
    {
        var employees = await employeeService.GetEmployees();
        if (Employees.Count > 0) {
            Employees.Clear();
        }

        foreach (var employee in employees) {
            Employees.Add(employee);
        }
    }

    [RelayCommand(CanExecute = nameof(CanAssignEmployee))]
    private async Task AssignEmployee()
    {
        bool success = await projectTaskService.AssignEmployeeToTask(SelectedTask.Id, SelectedEmployee!.Id);
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
               && (SelectedTask.AssignedEmployeeId != SelectedEmployee.Id);
    }
}