using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryProjectTaskRepository : IRepository<ProjectTask>
{
    private readonly List<ProjectTask> _tasks =
    [
        // new ProjectTask("Подготовка котлована",
        //     "Разработка и выемка грута", 1,40) { Id = GetNextId() },
        // new ProjectTask("Заливка фундамента",
        //     "Армирование и бетонирование плиты", 1) { Id = GetNextId() },
        //
        // new ProjectTask("Демонтажные работы",
        //     "Снятие старой отделки и перегородок", 2) { Id = GetNextId() },
        // new ProjectTask("Замена оконных блоков",
        //     "Установка новых ПВХ окон", 2) { Id = GetNextId() },
        //
        // new ProjectTask("Геодезические изыскания",
        //     "Разбивка трассы", 3) { Id = GetNextId() },
        // new ProjectTask("Устройство дорожной одежды",
        //     "Щебеночное основание", 3) { Id = GetNextId() },
        //
        // new ProjectTask("Разработка дизайн-проекта",
        //     null, 4) { Id = GetNextId() },
        // new ProjectTask("Закупка чистовых материалов",
        //     "Плитка, краска, ламинат", 4) { Id = GetNextId() }
    ];
    private static int _id = 1;

    private static int GetNextId()
    {
        return _id++;
    }

    public Task<ProjectTask?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default, params Expression<Func<ProjectTask, object>>[]? properties)
    {
        return Task.FromResult(_tasks.FirstOrDefault(e => e.Id == id));
    }
    public Task<IEnumerable<ProjectTask>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_tasks.AsEnumerable());
    }
    public Task<IEnumerable<ProjectTask>> GetAllAsync(Expression<Func<ProjectTask, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<ProjectTask, object>>[] properties)
    {
        return Task.FromResult(_tasks.AsEnumerable());
    }
    public Task AddAsync(ProjectTask entity, CancellationToken cancellationToken = default)
    {
        _tasks.Add(entity);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(ProjectTask entity, CancellationToken cancellationToken = default)
    {
        var employee = _tasks.FirstOrDefault(t => t.Id == entity.Id);
        if (employee != null) {
            _tasks.Remove(employee);
            _tasks.Add(entity);
        }

        return Task.CompletedTask;
    }
    public Task DeleteAsync(ProjectTask entity, CancellationToken cancellationToken = default)
    {
        var employee = _tasks.FirstOrDefault(m => m.Id == entity.Id);
        if (employee != null) {
            _tasks.Remove(employee);
        }
        return Task.CompletedTask;
    }
}