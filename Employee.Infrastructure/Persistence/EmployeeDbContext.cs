using Employee.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Persistence;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Employee> Employees => Set<Domain.Entities.Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EmployeeDbContext).Assembly);
    }
}
