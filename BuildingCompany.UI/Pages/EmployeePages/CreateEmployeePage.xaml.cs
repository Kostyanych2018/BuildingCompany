using BuildingCompany.UI.ViewModels.EmployeeViewModels;

namespace BuildingCompany.UI.Pages.EmployeePages;

public partial class CreateEmployeePage : ContentPage
{
    public CreateEmployeePage(CreateEmployeeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 