using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;

namespace BuildingCompany.UI.Pages.MaterialPages;

public partial class CreateMaterialPage : ContentPage
{
    private readonly CreateMaterialViewModel _viewModel;
    
    public CreateMaterialPage(CreateMaterialViewModel model)
    {
        InitializeComponent();
        BindingContext = _viewModel = model;
    }
    
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.CalculateFinalPriceCommand.Execute(null);
    }
    
    private void Category_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.CalculateFinalPriceCommand.Execute(null);
    }
}