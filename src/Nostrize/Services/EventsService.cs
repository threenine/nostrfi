using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nostrfi.Nostrize.Services;

public class EventsService
{
    private readonly IDbContextFactory<NostrfiDbContext> _dbContextFactory;
    private readonly ILogger<EventsService> _logger;

    public EventsService(IDbContextFactory<NostrfiDbContext> dbContextFactory, ILogger<EventsService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }
}