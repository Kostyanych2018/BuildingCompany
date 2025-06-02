using BuildingCompany.Domain.Abstractions;
using BuildingCompany.Domain.Entities;
using BuildingCompany.Infrastructure.Data;
using BuildingCompany.Infrastructure.Repositories;

namespace BuildingCompany.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Lazy<IRepository<Project>> _projectsRepository;
    private readonly Lazy<IRepository<ProjectTask>> _projectTaskRepository;
    private readonly Lazy<IRepository<Employee>> _employeesRepository;
    private readonly Lazy<IRepository<Material>> _materialsRepository;
    private readonly Lazy<IRepository<TaskMaterialRequirement>> _taskMaterialRequirementRepository;

    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
        _projectsRepository = new Lazy<IRepository<Project>>(() => new MongoEfRepository<Project>(_context));
        _projectTaskRepository = new Lazy<IRepository<ProjectTask>>(() => new MongoEfRepository<ProjectTask>(_context));
        _employeesRepository = new Lazy<IRepository<Employee>>(() => new MongoEfRepository<Employee>(_context));
        _materialsRepository = new Lazy<IRepository<Material>>(() => new MongoEfRepository<Material>(_context));
        _taskMaterialRequirementRepository = new Lazy<IRepository<TaskMaterialRequirement>>(() => new MongoEfRepository<TaskMaterialRequirement>(_context));
    }
    
    public IRepository<Project> ProjectsRepository => _projectsRepository.Value;
    public IRepository<ProjectTask> ProjectTaskRepository => _projectTaskRepository.Value;
    public IRepository<Employee> EmployeesRepository => _employeesRepository.Value;
    public IRepository<Material> MaterialsRepository => _materialsRepository.Value;
    public IRepository<TaskMaterialRequirement> TaskMaterialRequirementRepository => _taskMaterialRequirementRepository.Value;

    public async Task SaveAllAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CreateDatabaseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DeleteDatabaseAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}