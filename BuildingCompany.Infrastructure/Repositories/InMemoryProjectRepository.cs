using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryProjectRepository: IRepository<Project>
{
    private readonly List<Project> _projects;

    public InMemoryProjectRepository()
    {
        _projects =
        [
            // new Project("Строительство ЖК 'Солнечный'", "Первая очередь жилого комплекса",
            //     5e8m) { Id = 1 },
            //
            // new Project("Реконструкция школы \u21165", "Капитальный ремонт с расширением",
            //     15e7m) { Id = 2 },
            //
            // new Project("Строительство дороги М-11", "Новый участок федеральной трассы",
            //     12e8m) { Id = 3 },
            //
            // new Project("Ремонт офиса", "Внутренний ремонт административного здания", 5e6m)
            //     { Id = 4 }

        ];
    }


    public Task<Project?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default, params Expression<Func<Project, object>>[]? properties)
    {
        return Task.FromResult(_projects.FirstOrDefault(e => e.Id == id));
    }
    public Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_projects.AsEnumerable());
    }
    public Task<IEnumerable<Project>> GetAllAsync(Expression<Func<Project, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<Project, object>>[] properties)
    {
        return Task.FromResult(_projects.AsEnumerable());
    }
    public Task AddAsync(Project entity, CancellationToken cancellationToken = default)
    {
        _projects.Add(entity);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(Project entity, CancellationToken cancellationToken = default)
    {
        var employee = _projects.FirstOrDefault(t => t.Id == entity.Id);
        if (employee != null) {
            _projects.Remove(employee);
            _projects.Add(entity);
        }

        return Task.CompletedTask;
    }
    public Task DeleteAsync(Project entity, CancellationToken cancellationToken = default)
    {
        var employee = _projects.FirstOrDefault(m => m.Id == entity.Id);
        if (employee != null) {
            _projects.Remove(employee);
        }
        return Task.CompletedTask;
    }
}