using Microsoft.Extensions.DependencyInjection;

namespace BuildingCompany.Application;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var unitofWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var projects = new List<Project>()
        {
            new Project("Строительство ЖК 'Солнечный'",
                "Первая очередь жилого комплекса", 5e8m) ,
            new Project("Реконструкция школы \u21165",
                "Капитальный ремонт с расширением", 1e7m) ,
            new Project("Строительство дороги М-11",
                "Новый участок федеральной трассы", 12e8m) ,
        };
        foreach (var project in projects) {
            await unitofWork.ProjectsRepository.AddAsync(project);
        }

        var tasks = new List<ProjectTask>()
        {
            new ProjectTask("Подготовка котлована",
                "Разработка и выемка грута", projects[0].Id) ,
            new ProjectTask("Демонтажные работы",
                "Снятие старой отделки и перегородок", projects[1].Id) ,
            new ProjectTask("Устройство дорожной одежды",
                "Щебеночное основание", projects[2].Id) ,
        };
        foreach (var task in tasks) {
            await unitofWork.ProjectTaskRepository.AddAsync(task);
        }
        var employees = new List<Employee>()
        {
            new Employee("Иванов Иван", "Заглушка") ,
            new Employee("Петров Петр", "Заглушка") ,
            new Employee("Сидоров Сергей", "Заглушка") ,
        };
        foreach (var employee in employees) {
            await unitofWork.EmployeesRepository.AddAsync(employee);
        }

        await unitofWork.SaveAllAsync();
    }
}