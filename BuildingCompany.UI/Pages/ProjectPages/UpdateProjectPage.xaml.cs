using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.ProjectViewModels;

namespace BuildingCompany.UI.Pages.ProjectPages;

public partial class UpdateProjectPage : ContentPage
{
    private readonly UpdateProjectViewModel _viewModel;
    public UpdateProjectPage(UpdateProjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProjectCommand.ExecuteAsync(null);
    }
}