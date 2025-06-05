using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

public partial class CreateMaterialViewModel : ObservableObject
{
    private readonly IMaterialService _materialService;
    private readonly ImageService _imageService;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _materialName = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _unitOfMeasure = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))]
    private string _unitPrice = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(CreateMaterialCommand))] 
    private string _quantity = "";
    
    [ObservableProperty] 
    private bool _isPremium = false;
    
    [ObservableProperty] 
    private bool _isEcoFriendly = false;
    
    [ObservableProperty] 
    private bool _hasBulkDiscount = false;
    
    [ObservableProperty] 
    private string _finalPrice = "";
    
    [ObservableProperty] 
    private string _priceAdjustmentDescription = "Стандартная цена";
    
    [ObservableProperty] 
    private string _imagePath = "material_default.png";
    
    [ObservableProperty] 
    private string _secondaryImagePath = "";

    public CreateMaterialViewModel(IMaterialService materialService, ImageService imageService)
    {
        _materialService = materialService;
        _imageService = imageService;
    }

    [RelayCommand(CanExecute = nameof(CanCreateMaterial))]
    private async Task CreateMaterial()
    {
        decimal.TryParse(UnitPrice, out decimal unitPrice);
        int.TryParse(Quantity, out int quantity);
        
        string category = "Стандарт";
        if (IsPremium && IsEcoFriendly)
            category = "Премиум Эко";
        else if (IsPremium)
            category = "Премиум";
        else if (IsEcoFriendly)
            category = "Эко";
            
        var dto = new MaterialDto
        {
            Name = MaterialName,
            UnitOfMeasure = UnitOfMeasure,
            UnitPrice = unitPrice,
            Quantity = quantity,
            IsPremium = IsPremium,
            IsEcoFriendly = IsEcoFriendly,
            HasBulkDiscount = HasBulkDiscount,
            PriceAdjustmentDescription = PriceAdjustmentDescription,
            Category = category,
            ImagePath = _imageService.GetMaterialImage(category)
        };
        
        if (_imageService.HasSecondaryImage(category))
        {
            dto.SecondaryImagePath = _imageService.GetSecondaryMaterialImage(category);
        }
        
        var createdMaterial = await _materialService.CreateMaterial(dto);
        await Shell.Current.DisplayAlert("Успех", $"Материал '{createdMaterial.Name}' успешно создан.", "OK");
        MaterialName = "";
        UnitOfMeasure = "";
        UnitPrice = "";
        Quantity = "";
        IsPremium = false;
        IsEcoFriendly = false;
        HasBulkDiscount = false;
        FinalPrice = "";
        PriceAdjustmentDescription = "Стандартная цена";
        ImagePath = "material_default.png";
        SecondaryImagePath = "";
    }

    private bool CanCreateMaterial() =>
        !string.IsNullOrWhiteSpace(MaterialName) &&
        !string.IsNullOrWhiteSpace(UnitOfMeasure) &&
        decimal.TryParse(UnitPrice,out var unitPrice)
        && int.TryParse(Quantity, out int quantity)
        && quantity > 0 && unitPrice > 0;
    [RelayCommand]
    private void CalculateFinalPrice()
    {
        if (!decimal.TryParse(UnitPrice, out decimal unitPrice))
        {
            FinalPrice = "";
            PriceAdjustmentDescription = "Введите корректную цену";
            return;
        }
        
        decimal finalPrice = unitPrice;
        string description = "Стандартная цена";
        
        if (IsPremium)
        {
            finalPrice *= 1.20m; 
            description += " + Премиум надбавка (20%)";
        }
        
        if (IsEcoFriendly)
        {
            finalPrice *= 1.15m; 
            description += " + Эко надбавка (15%)";
        }
        
        if (HasBulkDiscount)
        {
            finalPrice *= 0.90m;
            description += " - Оптовая скидка (10%)";
        }
        
        FinalPrice = finalPrice.ToString("N2");
        PriceAdjustmentDescription = description;
        
        string category = "Стандарт";
        if (IsPremium && IsEcoFriendly)
            category = "Премиум Эко";
        else if (IsPremium)
            category = "Премиум";
        else if (IsEcoFriendly)
            category = "Эко";
            
        ImagePath = _imageService.GetMaterialImage(category);
        
        if (_imageService.HasSecondaryImage(category))
        {
            SecondaryImagePath = _imageService.GetSecondaryMaterialImage(category);
        }
        else
        {
            SecondaryImagePath = "";
        }
    }
}