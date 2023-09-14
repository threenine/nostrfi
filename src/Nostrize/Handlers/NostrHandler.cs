using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nostrfi.Nostrize.Handlers;

public class NostrHandler : WebSocketHandler
{
    private readonly ILogger<NostrHandler> _logger;

    public NostrHandler(ConnectionManager webSocketConnectionManager, ILogger<NostrHandler> logger) : base(webSocketConnectionManager)
    {
        _logger = logger;
    }

    public override async Task OnConnected(WebSocket socket)
    {
        await base.OnConnected(socket);
        var socketId = WebSocketConnectionManager.GetId(socket);
        _logger.LogInformation("WebSocket connection request: {SocketId}", socketId);
    
        
        
    }

    public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
    {
        var socketId = WebSocketConnectionManager.GetId(socket);
        var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
        
    }
}