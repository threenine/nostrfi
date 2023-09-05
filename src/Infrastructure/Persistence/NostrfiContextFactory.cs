using System.Diagnostics.CodeAnalysis;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nostrfi;

[ExcludeFromCodeCoverage]
internal class NostrfiContextFactory : IDesignTimeDbContextFactory<NostrfiDbContext>
{
    public NostrfiDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<NostrfiDbContext> dbContextOptionsBuilder =
            new();
        
        dbContextOptionsBuilder.UseNpgsql(ConnectionStringNames.LocalBuild);
        return new NostrfiDbContext(dbContextOptionsBuilder.Options);
    }
}