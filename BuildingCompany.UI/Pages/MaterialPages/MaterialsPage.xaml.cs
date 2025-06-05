using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;

namespace BuildingCompany.UI.Pages.MaterialPages;

public partial class MaterialsPage : ContentPage
{
    private readonly MaterialsViewModel _viewModel;
    public MaterialsPage(MaterialsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            await _viewModel.LoadMaterialsCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "OK");
        }
    }
}