using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

[QueryProperty(nameof(MaterialId), "materialId")]
public partial class MaterialDetailsViewModel(IMaterialService materialService) : ObservableObject
{
    [ObservableProperty] private ObjectId _materialId;

    [ObservableProperty] private MaterialDto? _material;

    [RelayCommand]
    private async Task LoadMaterial()
    {
        var material = await materialService.GetMaterial(MaterialId);
        if (material != null)
            Material = material;
    }
}