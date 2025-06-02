using System;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildingCompany.UI.ViewModels.EmployeeViewModels;

public partial class CreateEmployeeViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;

    [ObservableProperty] private string _fullName = string.Empty;
    [ObservableProperty] private string _position = string.Empty;
    [ObservableProperty] private int _experience = 0;
    [ObservableProperty] private int _certificationLevel = 1;

    public CreateEmployeeViewModel(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [RelayCommand]
    private void SetCertificationLevel(string level)
    {
        if (int.TryParse(level, out int certLevel) && certLevel >= 1 && certLevel <= 5)
        {
            CertificationLevel = certLevel;
        }
    }

    [RelayCommand]
    private async Task CreateEmployee()
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

        if (Experience < 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Опыт работы не может быть отрицательным", "OK");
            return;
        }

        if (CertificationLevel < 1 || CertificationLevel > 5)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Уровень сертификации должен быть от 1 до 5", "OK");
            return;
        }

        var employeeDto = new EmployeeDto
        {
            FullName = FullName,
            Position = Position,
            Experience = Experience,
            CertificationLevel = CertificationLevel,
            Status = "Available"
        };

        try
        {
            await _employeeService.CreateEmployee(employeeDto);
            await Shell.Current.DisplayAlert("Успех", "Сотрудник создан", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось создать сотрудника: {ex.Message}", "OK");
        }
    }
} 