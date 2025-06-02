using BuildingCompany.UI.ViewModels.EmployeeViewModels;

namespace BuildingCompany.UI.Pages.EmployeePages;

public partial class UpdateEmployeePage : ContentPage
{
    private readonly UpdateEmployeeViewModel _viewModel;
    
    public UpdateEmployeePage(UpdateEmployeeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEmployeeCommand.ExecuteAsync(null);
    }
} 