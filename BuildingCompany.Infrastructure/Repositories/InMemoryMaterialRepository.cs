using System.Linq.Expressions;
using MongoDB.Bson;

namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryMaterialRepository : IRepository<Material>
{
    private readonly List<Material> _materials =
    [
        // new Material("Кирпич керамический", "шт", 15.50m, 1e4m) { Id = _id++ },
        // new Material("Цемент", "кг", 0.8m, 5e3m) { Id = _id++ },
        // new Material("Песок строительный", "м3", 1m, 10000) { Id = _id++ },
        // new Material("Бетон", "м3", 10m, 6e3m) { Id = _id++ },
        
    ];

    private static int _id = 1;

    public Task<Material?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default, params Expression<Func<Material, object>>[]? properties)
    {
        return Task.FromResult(_materials.FirstOrDefault(m => m.Id == id));
    }
    public Task<IEnumerable<Material>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Material>>(_materials);
    }
    public Task<IEnumerable<Material>> GetAllAsync(Expression<Func<Material, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<Material, object>>[] properties)
    {
        return Task.FromResult<IEnumerable<Material>>(_materials);
    }
    public Task AddAsync(Material entity, CancellationToken cancellationToken = default)
    {
        _materials.Add(entity);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(Material entity, CancellationToken cancellationToken = default)
    {
        var material = _materials.FirstOrDefault(m => m.Id == entity.Id);
        if (material != null) {
            _materials.Remove(material);
            _materials.Add(entity);
        }
        return Task.CompletedTask;
    }
    public Task DeleteAsync(Material entity, CancellationToken cancellationToken = default)
    {
        var material = _materials.FirstOrDefault(m => m.Id == entity.Id);
        if (material != null) {
            _materials.Remove(material);
        }
        return Task.CompletedTask;
    }
}