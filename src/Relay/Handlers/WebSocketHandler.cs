using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;

namespace Nostrfi.Relay.Handlers;

public class WebSocketHandler
{
    private readonly ILogger<WebSocketHandler> _logger;
    public event EventHandler<string>? Connection;
    public WebSocketHandler(ILogger<WebSocketHandler> logger)
    {
        _logger = logger;
    }
    
    public virtual async Task OnConnected(WebSocket socket)
    {
        var connectionId = Guid.NewGuid().ToString().Replace("-", "");
        _logger.LogInformation("New websocket connection: {ConnectionId}", connectionId);
        Connection?.Invoke(this, connectionId);
    }
    
    public virtual Task OnDisconnected(WebSocket socket)
    {
        return Task.CompletedTask;
    }
    
    public virtual Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string msg)
    {
        return Task.CompletedTask;
    }
}