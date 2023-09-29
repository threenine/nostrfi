using Microsoft.Extensions.Hosting;

namespace Nostrfi.Nostrize.MessageHandlers;

/// <summary>
///  stop previous subscriptions
/// </summary>
public class CloseMessageHandler : IMessageHandler, IHostedService
{
    public  Task Handle(string connectionId, string msg)
    {
       return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}