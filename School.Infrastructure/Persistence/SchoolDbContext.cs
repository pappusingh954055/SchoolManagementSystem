using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public DbSet<School.Domain.Entities.School> Schools => Set<School.Domain.Entities.School>();

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.ApplyConfigurationsFromAssembly(
    //   typeof(SchoolDbContext).Assembly);
    //   modelBuilder.Entity<School.Domain.Entities.School>(entity =>
    //    {
    //        entity.HasKey(x => x.Id);

    //        entity.OwnsOne(x => x.Address, address =>
    //        {
    //            address.Property(a => a.Line1)
    //                .HasColumnName("Line1");

    //            address.Property(a => a.City)
    //                .HasColumnName("City");

    //            address.Property(a => a.State)
    //                .HasColumnName("State");

    //            address.Property(a => a.Country)
    //                .HasColumnName("Country");

    //            address.Property(a => a.PostalCode)
    //                .HasColumnName("PostalCode");
    //        });
    //    });
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<School.Domain.Entities.School>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.Name)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.PhotoUrl)
                  .HasMaxLength(500);

            entity.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Line1).HasColumnName("Line1").IsRequired();
                address.Property(a => a.City).HasColumnName("City").IsRequired();
                address.Property(a => a.State).HasColumnName("State").IsRequired();
                address.Property(a => a.Country).HasColumnName("Country").IsRequired();
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
            });
        });
    }
}
