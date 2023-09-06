using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nostrfi.Persistence.Models.Nostr;
using Threenine;
using Threenine.Configurations.PostgreSql;

namespace Nostrfi.Configurations;

public class EventsConfiguration : IEntityTypeConfiguration<Events>
{
    public void Configure(EntityTypeBuilder<Events> builder)
    {
        builder.ToTable(nameof(Events).ToLower());
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName(nameof(Events.Id).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
            .ValueGeneratedNever();
        
        builder.Property(e => e.PublicKey)
            .HasColumnName(nameof(Events.PublicKey).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
           .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .HasColumnName(nameof(Events.CreatedAt).ToSnakeCase())
            .HasColumnType(ColumnTypes.DateTimeOffSet)
            .IsRequired();
        
        builder.Property(e => e.Kind)
            .HasColumnName(nameof(Events.Kind).ToSnakeCase())
            .HasColumnType(ColumnTypes.Integer)
            .IsRequired();
        
        builder.Property(e => e.Content)
            .HasColumnName(nameof(Events.Content).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
            .IsRequired(false);
        
        builder.Property(e => e.Signature)
            .HasColumnName(nameof(Events.Signature).ToSnakeCase())
            .HasColumnType(ColumnTypes.Text)
            .IsRequired();

        builder.HasMany(e => e.Tags)
            .WithOne(t => t.Event)
            .HasForeignKey(t => t.EventId)
            .IsRequired();
    }
}