using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Mappings;
using MongoDB.Bson;

namespace BuildingCompany.Application.Services;

public class MaterialService(IUnitOfWork unitOfWork): IMaterialService
{
    /// <summary>
    /// Нужна проверка на существование такого же объекта
    /// </summary>
    public async Task<MaterialDto> CreateMaterial(MaterialDto materialDto)
    {
        var material = new Material(materialDto.Name, 
            materialDto.UnitOfMeasure,
            materialDto.UnitPrice,
            materialDto.Quantity);
        await unitOfWork.MaterialsRepository.AddAsync(material);
        await unitOfWork.SaveAllAsync();
        return material.ToDto();
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
        material.UpdateDetails(materialDto.Name, materialDto.UnitOfMeasure, materialDto.UnitPrice);
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