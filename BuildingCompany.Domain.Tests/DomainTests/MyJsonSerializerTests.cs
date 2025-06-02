// using BuildingCompany.Domain.Entities;
// using BuildingCompany.Serialization;
//
// namespace BuildingCompany.Domain.Tests.DomainTests;
//
// public class MyJsonSerializerTests
// {
//     private readonly MyJsonSerializer _serializer = new MyJsonSerializer();
//
//     [Fact]
//     public void SerializeAndDeserialize_Project_ShouldReturnEqualObject()
//     {
//         var initProject = new Project("Тестовый проект JSON", "Описание для JSON теста", 123m);
//         initProject.SetStatus(ProjectStatus.InProgress);
//         initProject.Tasks.Add(new ProjectTask("Задача 1 JSON", "Описание Задачи 1", initProject.Id));
//         initProject.Tasks.Add(new ProjectTask("Задача 2 JSON", "Описание Задачи 2", initProject.Id,50));
//         string jsonData = _serializer.Serialize(initProject);
//         Project? project = _serializer.Deserialize<Project>(jsonData);
//         
//         Assert.NotNull(project);
//         Assert.Equal(initProject, project);
//     }
// }