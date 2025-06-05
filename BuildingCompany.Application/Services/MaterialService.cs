using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using BuildingCompany.Domain.Abstractions;
using BuildingCompany.Domain.MaterialPricing;
using MongoDB.Bson;

namespace BuildingCompany.Application.Services;

public class MaterialService(IUnitOfWork unitOfWork): IMaterialService
{
    public async Task<MaterialDto> CreateMaterial(MaterialDto materialDto)
    {
        IMaterialPricing pricing = new BaseMaterialPricing();
        
        if (materialDto.IsPremium)
        {
            pricing = new PremiumMaterialPricing(pricing);
            materialDto.Category = "Премиум";
        }
        
        if (materialDto.IsEcoFriendly)
        {
            pricing = new EcoFriendlyMaterialPricing(pricing);
            materialDto.Category = materialDto.Category == "Премиум" ? "Премиум Эко" : "Эко";
        }
        
        if (materialDto.HasBulkDiscount)
        {
            pricing = new BulkDiscountPricing(pricing);
        }
        
        materialDto.FinalPrice = pricing.CalculatePrice(materialDto.UnitPrice);
        materialDto.PriceAdjustmentDescription = pricing.Description;
        
        var material = new Material(
            materialDto.Name, 
            materialDto.UnitOfMeasure,
            materialDto.UnitPrice,
            materialDto.FinalPrice,
            materialDto.Quantity,
            materialDto.Category);
            
        await unitOfWork.MaterialsRepository.AddAsync(material);
        await unitOfWork.SaveAllAsync();
        
        var resultDto = material.ToDto();
        resultDto.UnitPrice = materialDto.UnitPrice; 
        resultDto.FinalPrice = materialDto.FinalPrice; 
        resultDto.PriceAdjustmentDescription = materialDto.PriceAdjustmentDescription;
        resultDto.IsPremium = materialDto.IsPremium;
        resultDto.IsEcoFriendly = materialDto.IsEcoFriendly;
        resultDto.HasBulkDiscount = materialDto.HasBulkDiscount;
        
        return resultDto;
    }
    public async Task<IEnumerable<MaterialDto>> GetMaterials()
    {
        var materials = await unitOfWork.MaterialsRepository.GetAllAsync();
        return materials.ToDto();
    }
    public async Task<MaterialDto?> GetMaterial(ObjectId materialId)
    {
        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(materialId);
        return material?.ToDto();
    }
    public async Task<bool> UpdateMaterial(MaterialDto materialDto)
    {
        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(materialDto.Id);
        if(material == null) return false;
        material.UpdateDetails(materialDto.Name, materialDto.UnitOfMeasure, materialDto.Quantity);
        await unitOfWork.MaterialsRepository.UpdateAsync(material);
        await unitOfWork.SaveAllAsync();
        return true;
    }
    public async Task<bool> DeleteMaterial(ObjectId id)
    {
        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(id);
        if(material == null) return false;
        await unitOfWork.MaterialsRepository.DeleteAsync(material);
        await unitOfWork.SaveAllAsync();
        return true;
    }

    public async Task<bool> UpdateQuantity(MaterialDto materialDto)
    {
        var material = await unitOfWork.MaterialsRepository.GetByIdAsync(materialDto.Id);
        if(material == null) return false;
        if (materialDto.Quantity >= 0) {
            material.Add(materialDto.Quantity);
        }else if (materialDto.Quantity < 0) {
            material.Remove(materialDto.Quantity);
        }
        await unitOfWork.MaterialsRepository.UpdateAsync(material);
        await unitOfWork.SaveAllAsync();
        return true;
    }
}