using System.Linq.Expressions;
using BuildingCompany.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace BuildingCompany.Infrastructure.Repositories;

public class MongoEfRepository<T>: IRepository<T> where T: Entity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> Entities;

    public MongoEfRepository(AppDbContext context)
    {
        Context = context;
        Entities = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? properties)
    {
        IQueryable<T> query = Entities.AsQueryable();
        if (properties is not null && properties.Any()) {
            foreach (var property in properties) {
                query = query.Include(property);
            }
        }
        return await query.FirstOrDefaultAsync(e=>e.Id==id,cancellationToken);
    }
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Entities.ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] properties)
    {
        IQueryable<T> query = Entities.AsQueryable();
        if (properties.Any()) {
            foreach (Expression<Func<T, object>> property in properties) {
                query = query.Include(property);
            }
        }

        if (filter !=null) {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }
    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Remove(entity);
        return Task.CompletedTask;
    }
}