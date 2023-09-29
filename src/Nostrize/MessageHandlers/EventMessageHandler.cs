using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nostrfi.Nostrize.MessageHandlers;

/// <summary>
/// Publish events
/// </summary>
public class EventMessageHandler : IMessageHandler, IHostedService
{
    private readonly ILogger<EventMessageHandler> _logger;
    private readonly Channel<(string,string)> Pending = Channel.CreateUnbounded<(string, string)>();

    public EventMessageHandler(ILogger<EventMessageHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(string connectionId, string msg)
    {
        _logger.LogInformation("EventMessageHandler: {ConnectionId}, {Message}", connectionId, msg);
        if (!msg.StartsWith("[\"EVENT\"")) return;
       await Pending.Writer.WriteAsync((connectionId, msg));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Process(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    private async Task Process(CancellationToken cancellationToken)
    {
        while (await Pending.Reader.WaitToReadAsync(cancellationToken))
        {
            if (Pending.Reader.TryRead(out var evt))
            {
                _logger.LogInformation("ProcessEventMessages: {ConnectionId}, {Message}", evt.Item1, evt.Item2);
            }
        }
    }
}