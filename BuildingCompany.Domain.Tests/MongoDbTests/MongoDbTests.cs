using BuildingCompany.Domain.Entities;
using BuildingCompany.Domain.Strategies;
using BuildingCompany.Infrastructure.Data;
using BuildingCompany.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BuildingCompany.Domain.Tests.MongoDbTests;

public class MongoDbTests
{
    
    protected readonly AppDbContext _context;
    private readonly EfUnitOfWork _unitOfWork;
    protected readonly IQualificationStrategy _qualificationStrategy;

    public MongoDbTests()
    {
        var client = new MongoClient("mongodb://localhost:27017/");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMongoDB(client, "BuildingCompanyTestDb").Options;
        _context = new AppDbContext(options);
        _unitOfWork = new EfUnitOfWork(_context);
        _qualificationStrategy = new CertificationQualificationStrategy();
    }

    [Fact]
    public async Task ProjectsRepository_ShouldAddAndRetrieveProject()
    {
        await _unitOfWork.DeleteDatabaseAsync();
        await _unitOfWork.CreateDatabaseAsync();
        var project = new Project
        {
            Id = ObjectId.GenerateNewId(),
            Name = "Test Project",
            Description = "Test Description",
            Budget = 100000
        };
        await _unitOfWork.ProjectsRepository.AddAsync(project);
        await _unitOfWork.SaveAllAsync();
        var result = await _unitOfWork.ProjectsRepository.GetByIdAsync(project.Id);
        Assert.NotNull(result);
        Assert.Equal(project.Name, result.Name);
        Assert.Equal(project.Description, result.Description);
        Assert.Equal(project.Budget, result.Budget);
    }
    
    [Fact]
    public async Task EmployeesRepository_ShouldAddAndRetrieveEmployee()
    {
        var employee = new Employee
        {
            Id = ObjectId.GenerateNewId(),
            FullName = "Test Employee",
            Position = "Test Position"
        };
        
        await _unitOfWork.EmployeesRepository.AddAsync(employee);
        await _unitOfWork.SaveAllAsync();
        
        var result = await _unitOfWork.EmployeesRepository.GetByIdAsync(employee.Id);
        Assert.NotNull(result);
        Assert.Equal(employee.FullName, result.FullName);
        Assert.Equal(employee.Position, result.Position);
    }
    
    [Fact]
    public async Task MaterialsRepository_ShouldAddAndRetrieveMaterial()
    {
        var material = new Material
        {
            Id = ObjectId.GenerateNewId(),
            Name = "Test Material",
            UnitOfMeasure = "kg",
            UnitPrice = 10.5m,
            Quantity = 100
        };
        
        await _unitOfWork.MaterialsRepository.AddAsync(material);
        await _unitOfWork.SaveAllAsync();
        
        var result = await _unitOfWork.MaterialsRepository.GetByIdAsync(material.Id);
        Assert.NotNull(result);
        Assert.Equal(material.Name, result.Name);
        Assert.Equal(material.UnitOfMeasure, result.UnitOfMeasure);
        Assert.Equal(material.UnitPrice, result.UnitPrice);
    }
}