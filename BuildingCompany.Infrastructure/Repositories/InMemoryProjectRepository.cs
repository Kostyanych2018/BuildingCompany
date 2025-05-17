namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryProjectRepository: IRepository<Project>
{
    private readonly List<Project> _projects;

    public InMemoryProjectRepository()
    {
        _projects =
        [
            new Project("Строительство ЖК 'Солнечный'", "Первая очередь жилого комплекса",
                5e8m) { Id = 1 },

            new Project("Реконструкция школы \u21165", "Капитальный ремонт с расширением",
                15e7m) { Id = 2 },

            new Project("Строительство дороги М-11", "Новый участок федеральной трассы",
                12e8m) { Id = 3 },

            new Project("Ремонт офиса", "Внутренний ремонт административного здания", 5e6m)
                { Id = 4 }

        ];
    }


    public Task<Project?> GetByIdAsync(int id)
    {
        return Task.FromResult(_projects.FirstOrDefault(p => p.Id == id));
    }

    public Task<IEnumerable<Project>> GetAllAsync()
    {
        return Task.FromResult(_projects.AsEnumerable());
    }

    public Task AddAsync(Project entity)
    {
        _projects.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Project entity)
    {
        var project = _projects.FirstOrDefault(p => p.Id == entity.Id);
        if (project != null) {
            _projects.Remove(project);
            _projects.Add(entity);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Project entity)
    {
        _projects.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _projects.RemoveAll(p=>p.Id == id);
        return Task.CompletedTask;
    }
}