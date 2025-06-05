using BuildingCompany.UI.ViewModels.MaterialsViewModels;

namespace BuildingCompany.UI.Pages.MaterialPages;

public partial class UpdateMaterialPage : ContentPage
{
    private readonly UpdateMaterialViewModel _viewModel;
    
    public UpdateMaterialPage(UpdateMaterialViewModel viewModel)
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