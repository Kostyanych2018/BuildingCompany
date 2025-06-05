using System;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.EmployeeViewModels;

[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class UpdateEmployeeViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;
    private readonly ImageService _imageService;

    [ObservableProperty] private string _fullName = string.Empty;
    [ObservableProperty] private string _position = string.Empty;
    [ObservableProperty] private int _experience = 0;
    [ObservableProperty] private int _certificationLevel = 1;
    [ObservableProperty] private ObjectId _employeeId;
    [ObservableProperty] private string _status = string.Empty;
    [ObservableProperty] private string _imagePath = string.Empty;

    public UpdateEmployeeViewModel(IEmployeeService employeeService, ImageService imageService)
    {
        _employeeService = employeeService;
        _imageService = imageService;
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
            Experience = employee.Experience;
            CertificationLevel = employee.CertificationLevel;
            ImagePath = employee.ImagePath;
        }
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
            Id = EmployeeId,
            FullName = FullName,
            Position = Position,
            Status = Status,
            Experience = Experience,
            CertificationLevel = CertificationLevel,
            ImagePath = _imageService.GetEmployeeImage(Position)
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