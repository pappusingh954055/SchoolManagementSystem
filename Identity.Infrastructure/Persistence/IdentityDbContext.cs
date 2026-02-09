using Identity.Domain.Entities;
using Identity.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Domain.Users.UserRole> UserRoles => Set<Domain.Users.UserRole>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(IdentityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Identity.Domain.Common.AuditableEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Identity.Domain.Common.AuditableEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                // Ensure CreatedAt is set to current UTC time
                var createdAtProperty = entry.Property("CreatedAt");
                createdAtProperty.CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.SetModified();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
