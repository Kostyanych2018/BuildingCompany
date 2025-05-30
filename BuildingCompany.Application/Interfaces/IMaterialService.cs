using BuildingCompany.Application.DTOs;
using MongoDB.Bson;

namespace BuildingCompany.Application.Interfaces;

public interface IMaterialService
{
    Task<MaterialDto> CreateMaterial(MaterialDto materialDto); 
    Task<IEnumerable<MaterialDto>> GetMaterials();
    Task<MaterialDto?> GetMaterial(ObjectId materialId);
    Task<bool> UpdateMaterial(MaterialDto materialDto);
    Task<bool> DeleteMaterial(ObjectId id);
    Task<bool> UpdateQuantity(MaterialDto materialDto);
}