using BuildingCompany.UI.ViewModels.ProjectTaskViewModels;

namespace BuildingCompany.UI.Pages.ProjectTaskPages;

public partial class CreateTaskPage : ContentPage
{
    public CreateTaskPage(CreateTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 