using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Domain.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

public partial class EmployeesViewModel(IEmployeeService employeeService) : ObservableObject
{
    public ObservableCollection<EmployeeDto> Employees { get; } = [];

#pragma warning disable MVVMTK0045
    [ObservableProperty] private PlotModel _statusPlot = new();
#pragma warning restore MVVMTK0045

    [ObservableProperty] private EmployeeDto? _selectedEmployee;

    [RelayCommand]
    private async Task LoadEmployees()
    {
        var employeeDtos = (await employeeService.GetEmployees()).ToList();
        Employees.Clear();
        foreach (var dto in employeeDtos) {
            Employees.Add(dto);
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
            TitleFontSize = 14
        };

        var pieSeries = new PieSeries()
        {
            InsideLabelPosition = 0.8, Slices = new List<PieSlice>() ,
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