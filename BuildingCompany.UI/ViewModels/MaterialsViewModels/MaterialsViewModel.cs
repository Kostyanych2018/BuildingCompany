using System.Collections.ObjectModel;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.MaterialPages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

public partial class MaterialsViewModel(IMaterialService materialService) : ObservableObject
{
    public ObservableCollection<MaterialDto> Materials { get; set; } = [];
    
    [ObservableProperty] private MaterialDto? _selectedMaterial;
    [ObservableProperty] private string _message="";

    [RelayCommand]
    private async Task LoadMaterials()
    {
        var materialsDtos = (await materialService.GetMaterials()).ToList();
        Materials.Clear();
        foreach (var materialDto in materialsDtos) {
            Materials.Add(materialDto);
        }
    }

    [RelayCommand]
    private async Task GoToCreateMaterial()=>await Shell.Current.GoToAsync(nameof(CreateMaterialPage));

    [RelayCommand]
    private async Task GoToMaterialsDetails(MaterialDto materialDto)
    {
        IDictionary<string, object> dict = new Dictionary<string, object>()
        {
            { "materialId", materialDto.Id }
        };
        await Shell.Current.GoToAsync(nameof(MaterialDetailsPage), dict);
    }
    
 
    
    // [RelayCommand]
    
}