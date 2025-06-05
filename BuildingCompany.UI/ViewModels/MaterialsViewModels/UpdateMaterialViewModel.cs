using System.Globalization;
using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Bson;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

[QueryProperty(nameof(MaterialId), "materialId")]
public partial class UpdateMaterialViewModel : ObservableObject
{
    private readonly IMaterialService _materialService;

    [ObservableProperty] private ObjectId _materialId;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateMaterialCommand))]
    private string _materialName = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateMaterialCommand))]
    private string _unitOfMeasure = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateMaterialCommand))]
    private string _unitPrice = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UpdateMaterialCommand))]
    private string _quantity = "";

    [ObservableProperty] private MaterialDto? _material;

    public UpdateMaterialViewModel(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [RelayCommand]
    public async Task LoadMaterial()
    {
        try {
            Material = await _materialService.GetMaterial(MaterialId);
            if (Material != null) {
                MaterialName = Material.Name;
                UnitOfMeasure = Material.UnitOfMeasure;
                Quantity = Material.Quantity.ToString(CultureInfo.InvariantCulture);
            }
        }
        catch (Exception ex) {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить материал: {ex.Message}", "OK");
        }
    }

    [RelayCommand(CanExecute = nameof(CanUpdateMaterial))]
    private async Task UpdateMaterial()
    {
        try {
            int.TryParse(Quantity, out int quantity);

            if (Material == null) return;

            var updatedMaterial = new MaterialDto
            {
                Id = MaterialId,
                Name = MaterialName,
                UnitOfMeasure = UnitOfMeasure,
                Quantity = quantity,
            };

            bool result = await _materialService.UpdateMaterial(updatedMaterial);

            if (result) {
                await Shell.Current.DisplayAlert("Успех", $"Материал '{MaterialName}' успешно обновлен", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else {
                await Shell.Current.DisplayAlert("Ошибка", "Не удалось обновить материал", "OK");
            }
        }
        catch (Exception ex) {
            await Shell.Current.DisplayAlert("Ошибка", $"Ошибка при обновлении материала: {ex.Message}", "OK");
        }
    }

    private bool CanUpdateMaterial()
    {
        return !string.IsNullOrWhiteSpace(MaterialName)
               && !string.IsNullOrWhiteSpace(UnitOfMeasure)
               && int.TryParse(Quantity, out int quantity)
               && quantity > 0;
    }
}