using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Student.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student.Domain.Entities.Student>
{
    public void Configure(EntityTypeBuilder<Student.Domain.Entities.Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentCode)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Gender)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.DateOfBirth)
               .IsRequired();

        builder.Property(x => x.SchoolId)
               .IsRequired();

        builder.Property(x => x.PhotoUrl)
               .HasMaxLength(500).IsRequired(false);

        // ✅ Address as Value Object
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Line1)
                   .HasColumnName("Line1")
                   .IsRequired();

            address.Property(a => a.City)
                   .HasColumnName("City")
                   .IsRequired();

            address.Property(a => a.State)
                   .HasColumnName("State")
                   .IsRequired();

            address.Property(a => a.Country)
                   .HasColumnName("Country")
                   .IsRequired();

            address.Property(a => a.PostalCode)
                   .HasColumnName("PostalCode")
                   .IsRequired();
        });

        // Optional index for performance
        builder.HasIndex(x => x.StudentCode).IsUnique();
    }
}
