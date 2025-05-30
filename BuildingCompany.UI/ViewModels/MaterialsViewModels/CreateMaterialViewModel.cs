using BuildingCompany.Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

public partial class CreateMaterialViewModel(IMaterialService materialService) : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _materialName = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _unitOfMeasure = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _unitPrice = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))] 
    private string _quantity = "";

    [RelayCommand(CanExecute = nameof(CanCreateMaterial))]
    private async Task CreateMaterial()
    {
        decimal.TryParse(UnitPrice, out decimal unitPrice);
        decimal.TryParse(Quantity, out decimal quantity);
        var dto = new MaterialDto
        {
            Name = MaterialName,
            UnitOfMeasure = UnitOfMeasure,
            UnitPrice = unitPrice,
            Quantity = quantity,
        };
        var createdMaterial = await materialService.CreateMaterial(dto);
        await Shell.Current.DisplayAlert("Успех", $"Проект '{createdMaterial.Name}' успешно создан.", "OK");
        MaterialName = "";
        UnitOfMeasure = "";
        UnitPrice = "";
        Quantity = "";
    }

    private bool CanCreateMaterial()
    {
        return !string.IsNullOrWhiteSpace(MaterialName)
               && !string.IsNullOrWhiteSpace(UnitOfMeasure)
               && decimal.TryParse(UnitPrice, out decimal unitPrice)
               && decimal.TryParse(Quantity, out decimal quantity)
               && quantity > 0 && unitPrice > 0;
    }
}