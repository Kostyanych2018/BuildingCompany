namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryProjectTaskRepository : IRepository<ProjectTask>
{
    private readonly List<ProjectTask> _tasks =
    [
        new ProjectTask("Подготовка котлована",
            "Разработка и выемка грута", 1,40) { Id = GetNextId() },
        new ProjectTask("Заливка фундамента",
            "Армирование и бетонирование плиты", 1) { Id = GetNextId() },

        new ProjectTask("Демонтажные работы",
            "Снятие старой отделки и перегородок", 2) { Id = GetNextId() },
        new ProjectTask("Замена оконных блоков",
            "Установка новых ПВХ окон", 2) { Id = GetNextId() },

        new ProjectTask("Геодезические изыскания",
            "Разбивка трассы", 3) { Id = GetNextId() },
        new ProjectTask("Устройство дорожной одежды",
            "Щебеночное основание", 3) { Id = GetNextId() },

        new ProjectTask("Разработка дизайн-проекта",
            null, 4) { Id = GetNextId() },
        new ProjectTask("Закупка чистовых материалов",
            "Плитка, краска, ламинат", 4) { Id = GetNextId() }
    ];
    private static int _id = 1;

    private static int GetNextId()
    {
        return _id++;
    }

    public Task<ProjectTask?> GetByIdAsync(int id)
    {
        return Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public Task<IEnumerable<ProjectTask>> GetAllAsync()
    {
        return Task.FromResult(_tasks.AsEnumerable());
    }

    public Task AddAsync(ProjectTask entity)
    {
        _tasks.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ProjectTask entity)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == entity.Id);
        if (task != null) {
            _tasks.Remove(task);
            _tasks.Add(entity);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(ProjectTask entity)
    {
        _tasks.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _tasks.RemoveAll(p => p.Id == id);
        return Task.CompletedTask;
    }
}