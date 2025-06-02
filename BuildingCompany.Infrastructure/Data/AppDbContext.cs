using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BuildingCompany.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Material> Materials { get; set; }

    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Project>().ToCollection("projects");
        modelBuilder.Entity<ProjectTask>().ToCollection("tasks");
        modelBuilder.Entity<Employee>().ToCollection("employees");
        modelBuilder.Entity<Material>().ToCollection("materials");
        modelBuilder.Entity<TaskMaterialRequirement>().ToCollection("taskMaterialRequirements");

        modelBuilder.Entity<Project>().HasKey(p => p.Id);
        modelBuilder.Entity<ProjectTask>().HasKey(p => p.Id);
        modelBuilder.Entity<Employee>().HasKey(p => p.Id);
        modelBuilder.Entity<Material>().HasKey(p => p.Id);
        modelBuilder.Entity<TaskMaterialRequirement>().HasKey(p=>p.Id);
    }
}