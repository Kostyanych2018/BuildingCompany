using System.Collections.ObjectModel;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.UI.Pages.MaterialPages;
using BuildingCompany.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;

namespace BuildingCompany.UI.ViewModels.MaterialsViewModels;

public partial class MaterialsViewModel : ObservableObject
{
    private readonly IMaterialService _materialService;
    private readonly ImageService _imageService;
    
    public ObservableCollection<MaterialDto> Materials { get; set; } = [];
    
    [ObservableProperty] private MaterialDto? _selectedMaterial;
    [ObservableProperty] private string _message="";
    [ObservableProperty] private Chart _topMaterialsChart;

    public MaterialsViewModel(IMaterialService materialService, ImageService imageService)
    {
        _materialService = materialService;
        _imageService = imageService;
        
        TopMaterialsChart = new BarChart
        {
            LabelTextSize = 40,
            BackgroundColor = SKColors.Transparent
        };
    }

    [RelayCommand]
    private async Task LoadMaterials()
    {
        try
        {
            var materialsDtos = (await _materialService.GetMaterials()).ToList();
            Materials.Clear();
            foreach (var materialDto in materialsDtos) {
                materialDto.ImagePath = _imageService.GetMaterialImage(materialDto.Category);
                
                if (_imageService.HasSecondaryImage(materialDto.Category))
                {
                    materialDto.SecondaryImagePath = _imageService.GetSecondaryMaterialImage(materialDto.Category);
                }
                
                Materials.Add(materialDto);
            }
            
            UpdateTopMaterialsChart(materialsDtos);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить материалы: {ex.Message}", "OK");
        }
    }
    
    private void UpdateTopMaterialsChart(List<MaterialDto> materials)
    {
        var topMaterials = materials
            .OrderByDescending(m => m.FinalPrice)
            .Take(5)
            .ToList();
            
        if (topMaterials.Count == 0)
        {
            TopMaterialsChart = new BarChart
            {
                LabelTextSize = 40,
                BackgroundColor = SKColors.Transparent
            };
            return;
        }
        var entries = new List<Microcharts.ChartEntry>();
        
        foreach (var material in topMaterials)
        {
                
            entries.Add(new ChartEntry((float)material.FinalPrice)
            {
                Label = material.Name,
                ValueLabel = $"{material.FinalPrice:N2} BYN ",
                Color = GetCategoryColor(material.Category),
            });
        }
        TopMaterialsChart = new BarChart
        {
            Entries = entries,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelTextSize = 25,
            IsAnimated = false,
            ValueLabelTextSize = 25,
            BackgroundColor = SKColors.Transparent,
            LabelColor = SKColors.Black,
        };
    }
    
    private SKColor GetCategoryColor(string category)
    {
        return category switch
        {
           "Стандарт" => SKColors.Blue,
           "Премиум"=>SKColors.Goldenrod,
           "Премиум Эко"=>SKColors.Green,
           _ => SKColors.Black
        };
    }

    [RelayCommand]
    private async Task CreateMaterial()=>await Shell.Current.GoToAsync(nameof(CreateMaterialPage));

    [RelayCommand]
    private async Task MaterialsDetails(MaterialDto materialDto)
    {
        IDictionary<string, object> dict = new Dictionary<string, object>()
        {
            { "materialId", materialDto.Id }
        };
        await Shell.Current.GoToAsync(nameof(MaterialDetailsPage), dict);
    }
}