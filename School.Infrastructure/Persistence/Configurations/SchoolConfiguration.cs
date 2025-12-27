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

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);

        // ✅ VALUE OBJECT MAPPING (ONLY THIS)
        builder.OwnsOne(x => x.Code, cb =>
        {
            cb.Property(c => c.Value)
              .HasColumnName("Code")
              .IsRequired()
              .HasMaxLength(50);
        });
    }
}
