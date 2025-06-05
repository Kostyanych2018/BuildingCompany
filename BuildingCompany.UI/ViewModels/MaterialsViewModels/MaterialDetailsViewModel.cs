using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.MaterialPages;
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
    [RelayCommand]
    private async Task DeleteMaterial(MaterialDto materialDto)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Подтверждение", 
            $"Вы уверены, что хотите удалить материал '{materialDto.Name}'?", 
            "Да", "Отмена");
            
        if (!confirm) return;
        
        try
        {
            bool result = await materialService.DeleteMaterial(materialDto.Id);
            if (result)
            {
                await Shell.Current.DisplayAlert("Успех", "Материал успешно удален", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Не удалось удалить материал", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Ошибка при удалении материала: {ex.Message}", "OK");
        }
    }
    
    [RelayCommand]
    private async Task UpdateMaterial(MaterialDto materialDto)
    {
        IDictionary<string, object> dict = new Dictionary<string, object>()
        {
            { "materialId", materialDto.Id }
        };
        await Shell.Current.GoToAsync(nameof(UpdateMaterialPage), dict);
    }
}