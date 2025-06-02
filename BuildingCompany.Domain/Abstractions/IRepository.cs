using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BuildingCompany.Domain.Entities;
using MongoDB.Bson;

namespace BuildingCompany.Domain.Abstractions;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? properties);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] properties);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}