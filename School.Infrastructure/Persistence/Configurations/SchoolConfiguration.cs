using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<Domain.Entities.School>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.School> builder)
    {
        builder.ToTable("Schools");

        builder.HasKey(x => x.Id);

        // ---------------- Code (IDENTITY – STRING) ----------------
        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(50);

        // ---------------- Name ----------------
        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);

        // ---------------- Photo ----------------
        builder.Property(x => x.PhotoUrl)
               .HasMaxLength(500).IsRequired(false);

        // ---------------- Address (VALUE OBJECT) ----------------
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
    }
}
