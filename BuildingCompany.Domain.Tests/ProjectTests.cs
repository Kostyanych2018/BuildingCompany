using System;
using BuildingCompany.Domain.Entities;
using Xunit;

namespace BuildingCompany.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProject()
    {
        string name = "Новый ЖК 'Рассвет'";
        string? description = "Строительство жилого комплекса комфорт-класса";
        decimal budget = 50000000.00m;

        var project = new Project(name, description, budget);

        Assert.Equal(name, project.Name);
        Assert.Equal(description, project.Description);
        Assert.Equal(budget, project.Budget);
        Assert.Equal(ProjectStatus.Created, project.Status);
        Assert.NotNull(project.Tasks);
        Assert.Empty(project.Tasks);
    }

    [Fact]
    public void Constructor_WithNullDescription_ShouldCreateProject()
    {
        string name = "Реконструкция дороги";
        decimal budget = 1200000.00m;

        var project = new Project(name, null, budget);

        Assert.Equal(name, project.Name);
        Assert.Null(project.Description);
        Assert.Equal(budget, project.Budget);
        Assert.Equal(ProjectStatus.Created, project.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        string? description = "Описание";
        decimal budget = 10000.00m;

        var exception = Assert.Throws<ArgumentException>(() => new Project(invalidName, description, budget));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeBudget_ShouldThrowArgumentException()
    {
        string name = "Проект с ошибкой";
        string? description = "Описание";
        decimal invalidBudget = -100.00m;

        var exception = Assert.Throws<ArgumentException>(() => new Project(name, description, invalidBudget));
        Assert.Equal("budget", exception.ParamName);
    }

    [Fact]
    public void UpdateProject_WithValidData_ShouldUpdateProjectDetails()
    {
        var project = new Project("Старый Проект", "Старое Описание", 1000.00m);
        string newName = "Обновленный Проект";
        string? newDescription = "Новое описание проекта";
        decimal newBudget = 25000.00m;


        project.UpdateProject(newName, newDescription, newBudget);

        Assert.Equal(newName, project.Name);
        Assert.Equal(newDescription, project.Description);
        Assert.Equal(newBudget, project.Budget);
    }

    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProject_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        var project = new Project("Проект", "Описание", 1000.00m);
        string? validDescription = "Валидное описание";
        decimal validBudget = 5000.00m;

        
        var exception = Assert.Throws<ArgumentException>(() => project.UpdateProject(invalidName, validDescription, validBudget));
        Assert.Equal("name", exception.ParamName);
    }

    
    [Fact]
    public void UpdateProject_WithNegativeBudget_ShouldThrowArgumentException()
    {
        var project = new Project("Проект", "Описание", 1000.00m);
        string validName = "Валидное имя";
        string? validDescription = "Валидное описание";
        decimal invalidBudget = -500.00m;

        
        var exception = Assert.Throws<ArgumentException>(() => project.UpdateProject(validName, validDescription, invalidBudget));
        Assert.Equal("budget", exception.ParamName);
    }

    
    [Fact]
    public void SetStatus_WhenNewStatusIsDifferent_ShouldUpdateStatus()
    {
        var project = new Project("Проект", "Описание", 1000.00m); 
        ProjectStatus newStatus = ProjectStatus.InProgress;

        
        project.SetStatus(newStatus);

        Assert.Equal(newStatus, project.Status);
    }

    
    [Fact]
    public void SetStatus_WhenNewStatusIsSame_ShouldNotChangeStatus()
    {
        ProjectStatus initialStatus = ProjectStatus.Planned;
        var project = new Project("Проект", "Описание", 1000.00m);
        project.SetStatus(initialStatus); 

        
        project.SetStatus(initialStatus); 

        Assert.Equal(initialStatus, project.Status); 
    }

    
    [Fact]
    public void Tasks_IsInitializedAndEmpty_OnNewProject()
    {
        var project = new Project("Проект для задач", "Описание", 100.0m);

        
        Assert.NotNull(project.Tasks);
        Assert.Empty(project.Tasks);
    }
}