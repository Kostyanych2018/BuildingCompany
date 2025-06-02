using BuildingCompany.Application.DTOs;
using BuildingCompany.Application.Interfaces;
using BuildingCompany.Application.Services;
using BuildingCompany.Domain.Entities;
using BuildingCompany.Infrastructure.UnitOfWork;
using MongoDB.Bson;

namespace BuildingCompany.Domain.Tests.MongoDbTests;

public class MongoDbIntegrationTests : MongoDbTests
{
    private readonly IProjectService _projectService;
    private readonly IEmployeeService _employeeService;
    private readonly IMaterialService _materialService;
    private readonly IProjectTaskService _taskService;
    private readonly EfUnitOfWork _unitOfWork;

    public MongoDbIntegrationTests()
    {
        _unitOfWork = new EfUnitOfWork(_context);
        _projectService = new ProjectService(_unitOfWork);
        _materialService = new MaterialService(_unitOfWork);
        _taskService = new ProjectTaskService(_unitOfWork,_qualificationStrategy);
        _employeeService = new EmployeeService(_unitOfWork);
    }

    [Fact]
    public async Task CreateProject_ShouldCreateAndReturnProject()
    {
        var projectDto = new ProjectDto
        {
            Name = "Integration Test Project",
            Description = "Test Description",
            Budget = 150000
        };

        var result = await _projectService.CreateProject(projectDto);

        Assert.NotNull(result);
        Assert.NotEqual(ObjectId.Empty, result.Id);
        Assert.Equal(projectDto.Name, result.Name);
        Assert.Equal(projectDto.Description, result.Description);

        var savedProject = await _unitOfWork.ProjectsRepository.GetByIdAsync(result.Id);
        Assert.NotNull(savedProject);
        Assert.Equal(projectDto.Name, savedProject.Name);
    }

    [Fact]
    public async Task CreateMaterial_ShouldCreateAndReturnMaterial()
    {
        var materialDto = new MaterialDto
        {
            Name = "Test Material",
            UnitOfMeasure = "kg",
            UnitPrice = 10.5m,
            Quantity = 100
        };

        var result = await _materialService.CreateMaterial(materialDto);

        Assert.NotNull(result);
        Assert.NotEqual(ObjectId.Empty, result.Id);
        Assert.Equal(materialDto.Name, result.Name);
        Assert.Equal(materialDto.UnitOfMeasure, result.UnitOfMeasure);
        Assert.Equal(materialDto.UnitPrice, result.UnitPrice);

        var savedMaterial = await _unitOfWork.MaterialsRepository.GetByIdAsync(result.Id);
        Assert.NotNull(savedMaterial);
        Assert.Equal(materialDto.Name, savedMaterial.Name);
    }

    [Fact]
    public async Task GetProjects_ShouldReturnAllProjects()
    {
        var projects = new List<Project>
        {
            new Project
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Project 1",
                Description = "Description 1",
                Budget = 100000
            },
            new Project
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Project 2",
                Description = "Description 2",
                Budget = 200000
            }
        };

        foreach (var project in projects) {
            await _unitOfWork.ProjectsRepository.AddAsync(project);
        }

        await _unitOfWork.SaveAllAsync();

        var result = await _projectService.GetProjects();

        var projectDtos = result as ProjectDto[] ?? result.ToArray();
        Assert.Contains(projectDtos, p => p.Name == "Project 1");
        Assert.Contains(projectDtos, p => p.Name == "Project 2");
    }

    [Fact]
    public async Task ComplexScenario_CreateProjectWithTasksAndAssignEmployees()
    {
        var employee1 = new Employee() {Id = ObjectId.GenerateNewId( ),FullName = "Worker 1", Position = "Builder" };
        var employee2 = new Employee() {Id = ObjectId.GenerateNewId( ), FullName = "Worker 2", Position = "Electrician" };
        await _unitOfWork.EmployeesRepository.AddAsync(employee1);
        await _unitOfWork.EmployeesRepository.AddAsync(employee2);
        var material1Dto = new MaterialDto { Name = "Brick", UnitOfMeasure = "pcs", UnitPrice = 0.5m, Quantity = 1000 };
        var material2Dto = new MaterialDto { Name = "Cable", UnitOfMeasure = "m", UnitPrice = 2.0m, Quantity = 500 };

        var material1 = await _materialService.CreateMaterial(material1Dto);
        var material2 = await _materialService.CreateMaterial(material2Dto);

        var projectDto = new ProjectDto
        {
            Name = "Complex Project",
            Description = "Integration Test Project",
            Budget = 50000
        };

        var project = await _projectService.CreateProject(projectDto);

        var task1Dto = new ProjectTaskDto
        {
            Name = "Build Wall",
            Description = "Build brick wall",
            ProjectId = project.Id,
        };

        var task2Dto = new ProjectTaskDto
        {
            Name = "Install Wiring",
            Description = "Install electrical wiring",
            ProjectId = project.Id,
        };

        var task1 = await _taskService.CreateTask(task1Dto);
        var task2 = await _taskService.CreateTask(task2Dto);

        await _taskService.AssignEmployeeToTask(task1.Id, employee1.Id);
        await _taskService.AssignEmployeeToTask(task2.Id, employee2.Id);

        var projectTasks = await _taskService.GetTasksByProject(project.Id);
        Assert.Equal(2, projectTasks.Count());

        var retrievedTask1 = await _taskService.GetTask(task1.Id);
        var retrievedTask2 = await _taskService.GetTask(task2.Id);

        Assert.NotNull(retrievedTask1);
        Assert.NotNull(retrievedTask2);
        Assert.Equal(retrievedTask1.AssignedEmployeeId, employee1.Id);
        Assert.Equal(retrievedTask2.AssignedEmployeeId, employee2.Id);
    }
}