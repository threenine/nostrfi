using Microsoft.EntityFrameworkCore;

namespace Nostrfi.Relay.Services;

/// <summary>
/// Manages database migrations to ensure the database is up to date with the latest version of the application.
/// </summary>
public class MigrationsHostedService : IHostedService
{
    private readonly IDbContextFactory<NostrfiDbContext> _factory;
    private readonly ILogger<MigrationsHostedService> _logger;

    public MigrationsHostedService(IDbContextFactory<NostrfiDbContext> factory, ILogger<MigrationsHostedService> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("{MigrationsHostedService} attempting to migrate database to latest version", nameof(MigrationsHostedService));
            
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
               var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);

               var migrations = pendingMigrations as string[] ?? pendingMigrations.ToArray();
               _logger.LogInformation("{MigrationsHostedService} pending migrations: {PendingMigrations}", nameof(MigrationsHostedService), migrations.Any() ? string.Join(", ", migrations) : "none");
               
               await context.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{MigrationsHostedService} error on the Migrations Startup Task", nameof(MigrationsHostedService));
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{MigrationHostedService} stopped", nameof(MigrationsHostedService));
        await Task.CompletedTask;
    }
}