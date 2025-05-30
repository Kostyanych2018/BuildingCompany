using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;

namespace BuildingCompany.UI.Pages.MaterialPages;

public partial class MaterialDetailsPage : ContentPage
{
    private readonly MaterialDetailsViewModel _viewModel;
    public MaterialDetailsPage(MaterialDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMaterialCommand.ExecuteAsync(null);
    }
}