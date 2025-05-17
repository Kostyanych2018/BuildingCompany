using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Tests;

public class ProjectTaskTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProjectTask()
    {
        string name = "Разработка котлована";
        string? description = "Выемка грунта под фундамент";
        int projectId = 1;
        int completionPercentage = 0;

        var task = new ProjectTask(name, description, projectId, completionPercentage);

        Assert.Equal(name, task.Name);
        Assert.Equal(description, task.Description);
        Assert.Equal(projectId, task.ProjectId);
        Assert.Equal(ProjectTaskStatus.Created, task.Status);
        Assert.Equal(completionPercentage, task.CompletionPercentage);
        Assert.Null(task.AssignedEmployeeId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentNullException(string invalidName)
    {
        string? description = "Описание задачи";
        int projectId = 1;

        var exception = Assert.Throws<ArgumentNullException>(() => new ProjectTask(invalidName, description, projectId));
        Assert.Equal("name", exception.ParamName);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidProjectId_ShouldThrowArgumentOutOfRangeException(int invalidProjectId)
    {
        string name = "Задача без проекта";
        string? description = "Описание";

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ProjectTask(name, description, invalidProjectId));
        Assert.Equal("projectId", exception.ParamName);
    }
    
    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdateTaskDetails()
    {
        var task = new ProjectTask("Старая задача", "Старое описание", 1);
        string newName = "Новая задача";
        string? newDescription = "Новое описание задачи";

        task.UpdateDetails(newName, newDescription);

        Assert.Equal(newName, task.Name);
        Assert.Equal(newDescription, task.Description);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void UpdateCompletionPercentage_WithValidPercentage_ShouldUpdatePercentage(int percentage)
    {
        var task = new ProjectTask("Задача", "Описание", 1);

        task.UpdateCompletionPercentage(percentage);

        Assert.Equal(percentage, task.CompletionPercentage);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void UpdateCompletionPercentage_WithInvalidPercentage_ShouldThrowArgumentOutOfRangeException(int invalidPercentage)
    {
        var task = new ProjectTask("Задача", "Описание", 1);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => task.UpdateCompletionPercentage(invalidPercentage));
        Assert.Equal("percentage", exception.ParamName);
    }
    
    [Fact]
    public void SetStatus_WhenNewStatusIsDifferent_ShouldUpdateStatus()
    {
        var task = new ProjectTask("Задача", "Описание", 1); 
        ProjectTaskStatus newStatus = ProjectTaskStatus.InProgress;

        task.SetStatus(newStatus);

        Assert.Equal(newStatus, task.Status);
    }
    
    [Fact]
    public void SetStatus_WhenNewStatusIsSame_ShouldNotChangeStatus()
    {
        ProjectTaskStatus initialStatus = ProjectTaskStatus.InProgress;
        var task = new ProjectTask("Задача", "Описание", 1);
        task.SetStatus(initialStatus); 
        
        task.SetStatus(initialStatus); 

        Assert.Equal(initialStatus, task.Status); 
    }
    
    [Fact]
    public void AssignEmployee_WithValidEmployee_ShouldSetAssignedEmployeeId()
    {
        var task = new ProjectTask("Задача", "Описание", 1);
        var employee = new Employee("Тест Сотрудник", "Профессия") { Id = 10 }; // Используем конструктор с ID, если Entity позволяет

        task.AssignEmployee(employee);

        Assert.Equal(employee.Id, task.AssignedEmployeeId);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void AssignEmployee_WithInvalidEmployeeId_ShouldThrowArgumentOutOfRangeException(int invalidEmployeeId)
    {
        var task = new ProjectTask("Задача", "Описание", 1);
        var invalidEmployee = new Employee("Имя", "Должность") { Id = invalidEmployeeId };

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => task.AssignEmployee(invalidEmployee));
        Assert.Equal("Id", exception.ParamName);
    }
}
