using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryEmployeeRepository : IRepository<Employee>
{
    private static int _id = 1;

    private readonly List<Employee> _employees =
    [
        // new Employee("Иванов Иван", "Прораб") { Id = _id++ },
        // new Employee("Петров Петр","Каменщик") { Id = _id++ },
        // new Employee("Сидоров Сергей","Сварщик") { Id = _id++ },
        // new Employee("Кузнецов Дмитрий","Водитель") { Id = _id++ },
    ];

    public Task<Employee?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default, params Expression<Func<Employee, object>>[]? properties)
    {
        return Task.FromResult(_employees.FirstOrDefault(e => e.Id == id));
    }
    public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_employees.AsEnumerable());
    }
    public Task<IEnumerable<Employee>> GetAllAsync(Expression<Func<Employee, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<Employee, object>>[] properties)
    {
        return Task.FromResult(_employees.AsEnumerable());
    }
    public Task AddAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        _employees.Add(entity);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        var employee = _employees.FirstOrDefault(t => t.Id == entity.Id);
        if (employee != null) {
            _employees.Remove(employee);
            _employees.Add(entity);
        }

        return Task.CompletedTask;
    }
    public Task DeleteAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        var employee = _employees.FirstOrDefault(m => m.Id == entity.Id);
        if (employee != null) {
            _employees.Remove(employee);
        }
        return Task.CompletedTask;
    }
}