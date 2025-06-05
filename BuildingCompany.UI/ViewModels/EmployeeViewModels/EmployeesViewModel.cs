using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Domain.Abstractions;
using BuildingCompany.UI.Pages.EmployeePages;
using BuildingCompany.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using Legend = OxyPlot.Legends.Legend;
using LegendPosition = OxyPlot.Legends.LegendPosition;

namespace BuildingCompany.UI.ViewModels.EmployeeViewModels;

public class EmployeeStatusCount
{
    public string Status { get; set; } = "";
    public int Count { get; set; }
}

public partial class EmployeesViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;
    private readonly ImageService _imageService;

    public ObservableCollection<EmployeeDto> Employees { get; } = [];

#pragma warning disable MVVMTK0045
    [ObservableProperty] private PlotModel _statusPlot = new();
#pragma warning restore MVVMTK0045

    [ObservableProperty] private EmployeeDto? _selectedEmployee;
    [ObservableProperty] private string _message = string.Empty;

    public EmployeesViewModel(IEmployeeService employeeService,ImageService imageService)
    {
        _employeeService = employeeService;
        _imageService = imageService;
    }

    [RelayCommand]
    private async Task LoadEmployees()
    {
        var employeeDtos = (await _employeeService.GetEmployees()).ToList();
        Employees.Clear();
        foreach (var dto in employeeDtos) {
            dto.ImagePath = _imageService.GetEmployeeImage(dto.Position);
            Employees.Add(dto);
        }
        
        if (employeeDtos.Count == 0)
        {
            Message = "Сотрудников пока нет";
        }
        else
        {
            Message = string.Empty;
        }
    }
    
    [RelayCommand]
    private async Task GoToCreateEmployee() => await Shell.Current.GoToAsync(nameof(CreateEmployeePage));
    
    [RelayCommand]
    private async Task UpdateEmployee()
    {
        if (SelectedEmployee == null) return;
        
        var parameters = new Dictionary<string, object>
        {
            { "employeeId", SelectedEmployee.Id }
        };
        
        await Shell.Current.GoToAsync(nameof(UpdateEmployeePage), parameters);
    }
    
    [RelayCommand]
    private async Task DeleteEmployee()
    {
        if (SelectedEmployee == null) return;
        
        bool confirmed = await Shell.Current.DisplayAlert(
            "Удаление",
            $"Удалить сотрудника '{SelectedEmployee.FullName}'?",
            "Да", "Нет");
        
        if (!confirmed) return;
        
        var assignedTask = await _employeeService.GetAssignedTask(SelectedEmployee.Id);
        if (assignedTask != null)
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                $"Невозможно удалить сотрудника, так как он назначен на задачу '{assignedTask.Name}'",
                "OK");
            return;
        }
        
        bool success = await _employeeService.DeleteEmployee(SelectedEmployee.Id);
        if (success)
        {
            await Shell.Current.DisplayAlert(
                "Удаление",
                $"Сотрудник '{SelectedEmployee.FullName}' удален",
                "OK");
                
            Employees.Remove(SelectedEmployee);
            BuildStatusPlot();
        }
        else
        {
            await Shell.Current.DisplayAlert(
                "Ошибка",
                "Не удалось удалить сотрудника",
                "OK");
        }
    }
    
    public void BuildStatusPlot()
    {
        var statusCounts = Employees.GroupBy(e => e.Status)
            .Select(g => new EmployeeStatusCount
            {
                Status = g.Key,
                Count = g.Count()
            }).ToList();

        var plotModel = new PlotModel()
        {
            Title = "Занятость сотрудников по статусам", 
            TitleFontSize = 14,
            Padding = new OxyThickness(0,10,0,0),
        };

        var pieSeries = new PieSeries()
        {
            InsideLabelPosition = 0.8, 
            Slices = new List<PieSlice>(),
            LabelField = "{1}",
            OutsideLabelFormat = "{0}"
        };
        
        foreach (var item in statusCounts) {
            var slice = new PieSlice(item.Status, item.Count);
            pieSeries.Slices.Add(slice);
        }

        plotModel.Series.Add(pieSeries);
        StatusPlot = plotModel;
        OnPropertyChanged(nameof(StatusPlot));
    }
}