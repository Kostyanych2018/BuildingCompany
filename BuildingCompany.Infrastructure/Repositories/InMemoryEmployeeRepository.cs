namespace BuildingCompany.Infrastructure.Repositories;

public class InMemoryEmployeeRepository : IRepository<Employee>
{
    private static int _id = 1;

    private readonly List<Employee> _employees =
    [
        new Employee("Иванов Иван", "Прораб") { Id = _id++ },
        new Employee("Петров Петр","Каменщик") { Id = _id++ },
        new Employee("Сидоров Сергей","Сварщик") { Id = _id++ },
        new Employee("Кузнецов Дмитрий","Водитель") { Id = _id++ },
    ];

    public Task<Employee?> GetByIdAsync(int id)
    {
        return Task.FromResult(_employees.FirstOrDefault(e => e.Id == id));
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        return Task.FromResult(_employees.AsEnumerable());
    }

    public Task AddAsync(Employee entity)
    {
        _employees.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Employee entity)
    {
        var employee = _employees.FirstOrDefault(t => t.Id == entity.Id);
        if (employee != null) {
            _employees.Remove(employee);
            _employees.Add(entity);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee entity)
    {
        _employees.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _employees.RemoveAll(e=>e.Id == id);
        return Task.CompletedTask;
    }
}