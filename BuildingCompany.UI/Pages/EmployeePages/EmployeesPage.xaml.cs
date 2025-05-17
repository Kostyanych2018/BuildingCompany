using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.EmployeeViewModels;
using Microsoft.Maui.Controls;

namespace BuildingCompany.UI.Pages.EmployeePages;

public partial class EmployeesPage : ContentPage
{
    private readonly EmployeesViewModel _viewModel;

    public EmployeesPage(EmployeesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        try {
            base.OnAppearing();
            if (_viewModel.LoadEmployeesCommand.CanExecute(null)) {
                await _viewModel.LoadEmployeesCommand.ExecuteAsync(null);
            }
            _viewModel.BuildStatusPlot();
        }
        catch (Exception e) {
            throw new Exception(e.Message);
        }
    }
}