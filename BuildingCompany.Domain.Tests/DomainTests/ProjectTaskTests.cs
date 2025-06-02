// using BuildingCompany.Domain.Entities;
// using MongoDB.Bson;
//
// namespace BuildingCompany.Domain.Tests.DomainTests;
//
// public class ProjectTaskTests
// {
//     [Fact]
//     public void Constructor_WithValidData_ShouldCreateProjectTask()
//     {
//         string name = "Разработка котлована";
//         string? description = "Выемка грунта под фундамент";
//         ObjectId projectId = ObjectId.GenerateNewId();
//         int completionPercentage = 0;
//
//         var task = new ProjectTask(name, description, projectId, completionPercentage);
//
//         Assert.Equal(name, task.Name);
//         Assert.Equal(description, task.Description);
//         Assert.Equal(projectId, task.ProjectId);
//         Assert.Equal(ProjectTaskStatus.Created, task.Status);
//         Assert.Equal(completionPercentage, task.CompletionPercentage);
//         Assert.Null(task.AssignedEmployeeId);
//     }
//
//     [Theory]
//     [InlineData(null)]
//     [InlineData("")]
//     [InlineData("   ")]
//     public void Constructor_WithInvalidName_ShouldThrowArgumentNullException(string invalidName)
//     {
//         string? description = "Описание задачи";
//         ObjectId projectId = ObjectId.Empty;
//
//         var exception = Assert.Throws<ArgumentNullException>(() => new ProjectTask(invalidName, description, projectId));
//         Assert.Equal("name", exception.ParamName);
//     }
//     
//     [Fact]
//     public void UpdateDetails_WithValidData_ShouldUpdateTaskDetails()
//     {
//         var task = new ProjectTask("Старая задача", "Старое описание", ObjectId.Empty);
//         string newName = "Новая задача";
//         string? newDescription = "Новое описание задачи";
//
//         task.UpdateDetails(newName, newDescription);
//
//         Assert.Equal(newName, task.Name);
//         Assert.Equal(newDescription, task.Description);
//     }
//     
//     [Theory]
//     [InlineData(0)]
//     [InlineData(50)]
//     [InlineData(100)]
//     public void UpdateCompletionPercentage_WithValidPercentage_ShouldUpdatePercentage(int percentage)
//     {
//         var task = new ProjectTask("Задача", "Описание", ObjectId.Empty);
//
//         task.UpdateCompletionPercentage(percentage);
//
//         Assert.Equal(percentage, task.CompletionPercentage);
//     }
//     [Theory]
//     [InlineData(-1)]
//     [InlineData(101)]
//     public void UpdateCompletionPercentage_WithInvalidPercentage_ShouldThrowArgumentOutOfRangeException(int invalidPercentage)
//     {
//         var task = new ProjectTask("Задача", "Описание", ObjectId.Empty);
//
//         var exception = Assert.Throws<ArgumentOutOfRangeException>(() => task.UpdateCompletionPercentage(invalidPercentage));
//         Assert.Equal("percentage", exception.ParamName);
//     }
//     
//     [Fact]
//     public void SetStatus_WhenNewStatusIsDifferent_ShouldUpdateStatus()
//     {
//         var task = new ProjectTask("Задача", "Описание", ObjectId.Empty); 
//         ProjectTaskStatus newStatus = ProjectTaskStatus.InProgress;
//
//         task.SetStatus(newStatus);
//
//         Assert.Equal(newStatus, task.Status);
//     }
//     
//     [Fact]
//     public void SetStatus_WhenNewStatusIsSame_ShouldNotChangeStatus()
//     {
//         ProjectTaskStatus initialStatus = ProjectTaskStatus.InProgress;
//         var task = new ProjectTask("Задача", "Описание", ObjectId.Empty);
//         task.SetStatus(initialStatus); 
//         
//         task.SetStatus(initialStatus); 
//
//         Assert.Equal(initialStatus, task.Status); 
//     }
//     
//  
// }
