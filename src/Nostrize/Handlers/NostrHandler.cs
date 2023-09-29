using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Nostrfi.Nostrize.MessageHandlers;

namespace Nostrfi.Nostrize.Handlers;

public class NostrHandler : WebSocketHandler
{
    private readonly ILogger<NostrHandler> _logger;
    private readonly IEnumerable<IMessageHandler> _messageHandlers;

    public NostrHandler(ConnectionManager webSocketConnectionManager, ILogger<NostrHandler> logger, IEnumerable<IMessageHandler> messageHandlers) : base(webSocketConnectionManager)
    {
        _logger = logger;
        _messageHandlers = messageHandlers;
    }

    public override async Task OnConnected(WebSocket socket)
    {
        await base.OnConnected(socket);
        var socketId = WebSocketConnectionManager.GetId(socket);
        _logger.LogInformation("WebSocket connection request: {SocketId}", socketId);
    }

    public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string buffer)
    {
        var socketId = WebSocketConnectionManager.GetId(socket);
        var message = buffer;
        
        _logger.LogInformation("Received message from {SocketId}: {Message}", socketId, message);
        
        return Task.WhenAll(_messageHandlers.AsParallel().Select(handler => handler.Handle(socketId, message)));
      
        
    }
}