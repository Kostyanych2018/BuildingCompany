using System;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.EmployeeViewModels;

[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class UpdateEmployeeViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;

    [ObservableProperty] private string _fullName = string.Empty;
    [ObservableProperty] private string _position = string.Empty;
    [ObservableProperty] private ObjectId _employeeId;
    [ObservableProperty] private string _status = string.Empty;

    public UpdateEmployeeViewModel(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [RelayCommand]
    private async Task LoadEmployee()
    {
        var employee = await _employeeService.GetEmployee(EmployeeId);
        if (employee != null)
        {
            FullName = employee.FullName;
            Position = employee.Position;
            Status = employee.Status;
        }
    }

    [RelayCommand]
    private async Task UpdateEmployee()
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            await Shell.Current.DisplayAlert("Ошибка", "ФИО сотрудника не может быть пустым", "OK");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(Position))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Должность не может быть пустой", "OK");
            return;
        }

        var employeeDto = new EmployeeDto
        {
            Id = EmployeeId,
            FullName = FullName,
            Position = Position,
            Status = Status
        };

        try
        {
            bool success = await _employeeService.UpdateEmployee(employeeDto);
            if (success)
            {
                await Shell.Current.DisplayAlert("Успех", "Данные сотрудника обновлены", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Не удалось обновить данные сотрудника", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось обновить данные сотрудника: {ex.Message}", "OK");
        }
    }
} 