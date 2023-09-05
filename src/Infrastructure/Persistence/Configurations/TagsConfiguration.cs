using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;
using Threenine;
using Threenine.Configurations.PostgreSql;

namespace Nostrfi.Configurations;

public class TagsConfiguration : IEntityTypeConfiguration<Tags>
{
    public void Configure(EntityTypeBuilder<Tags> builder)
    {
        builder.ToTable(nameof(Tags).ToLower());
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName(nameof(Tags.Id).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
            .ValueGeneratedNever();

        builder.Property(e => e.EventId)
            .HasColumnName(nameof(Tags.EventId).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
            .IsRequired();

        builder.Property(e => e.Tag)
            .HasColumnName(nameof(Tags.Tag).ToSnakeCase())
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName(nameof(Tags.Value).ToSnakeCase())
            .IsRequired();
        
      


    }
}