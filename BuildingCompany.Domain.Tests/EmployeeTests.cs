using BuildingCompany.Domain.Entities;

namespace BuildingCompany.Domain.Tests;

public class EmployeeTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateEmployee()
    {
        string fullName = "Иван Иванов";
        string position = "Прораб";
        EmployeeStatus status = EmployeeStatus.Available;
        var employee = new Employee(fullName, position, status);
        Assert.Equal(fullName, employee.FullName);
        Assert.Equal(position, employee.Position);
        Assert.Equal(status, employee.Status);
        Assert.Null(employee.AssignedTaskId);
    }

    [Fact]
    public void Constructor_WithValidDataAndDefaultStatus_ShouldCreateEmployeeWithAvailableStatus()
    {
        string fullName = "Петр Петров";
        string position = "Каменщик";
        var employee = new Employee(fullName, position);
        Assert.Equal(fullName, employee.FullName);
        Assert.Equal(position, employee.Position);
        Assert.Equal(EmployeeStatus.Available, employee.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidFullName_ShouldThrowArgumentException(string invalidFullName)
    {
        string position = "Прораб";
        var exception = Assert.Throws<ArgumentException>(() => new Employee(invalidFullName, position));
        Assert.Equal("fullName", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidPosition_ShouldThrowArgumentException(string invalidPosition)
    {
        string fullName = "Иван Иванов";
        var exception = Assert.Throws<ArgumentException>(() => new Employee(fullName, invalidPosition));
        Assert.Equal("position", exception.ParamName);
    }


    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdateFullNameAndPosition()
    {
        var employee = new Employee("Старое Имя", "Старая Должность");
        string newFullName = "Новое Имя";
        string newPosition = "Новая Должность";

        employee.UpdateDetails(newFullName, newPosition);

        Assert.Equal(newFullName, employee.FullName);
        Assert.Equal(newPosition, employee.Position);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithInvalidFullName_ShouldThrowArgumentException(string invalidFullName)
    {
        var employee = new Employee("Иван Иванов", "Прораб");
        string validPosition = "Инженер";

        var exception = Assert.Throws<ArgumentException>(() => employee.UpdateDetails(invalidFullName, validPosition));
        Assert.Equal("fullName", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithInvalidPosition_ShouldThrowArgumentException(string invalidPosition)
    {
        var employee = new Employee("Иван Иванов", "Прораб");
        string validFullName = "Петр Петров";

        var exception = Assert.Throws<ArgumentException>(() => employee.UpdateDetails(validFullName, invalidPosition));
        Assert.Equal("position", exception.ParamName);
    }

    [Fact]
    public void SetStatus_WhenNewStatusIsDifferent_ShouldUpdateStatus()
    {
        var employee = new Employee("Иван Иванов", "Прораб", EmployeeStatus.Available);
        EmployeeStatus newStatus = EmployeeStatus.Busy;

        employee.SetStatus(newStatus);

        Assert.Equal(newStatus, employee.Status);
    }

    [Fact]
    public void SetStatus_WhenNewStatusIsSame_ShouldNotChangeStatus()
    {
        EmployeeStatus initialStatus = EmployeeStatus.Busy;
        var employee = new Employee("Иван Иванов", "Прораб", initialStatus);

        employee.SetStatus(initialStatus);

        Assert.Equal(initialStatus, employee.Status);
    }

    [Fact]
    public void AssignTask_WithValidTask_ShouldSetAssignedTaskId()
    {
        
        var employee = new Employee("Иван Иванов", "Прораб");
        var task = new ProjectTask("Тестовая задача", "Описание", 1) { Id = 123 }; 
        employee.AssignTask(task);
        Assert.Equal(task.Id, employee.AssignedTaskId);
    }
}