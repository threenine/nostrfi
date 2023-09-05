using System.Reflection;
using Database;
using Microsoft.EntityFrameworkCore;
using Threenine.Configurations.PostgreSql;

namespace Nostrfi;

public class NostrfiDbContext : DbContext
{
    public NostrfiDbContext(DbContextOptions<NostrfiDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema.Name);
        modelBuilder.HasPostgresExtension(PostgreExtensions.UUIDGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}