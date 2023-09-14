using System.Net.WebSockets;

namespace Nostrfi.Nostrize.Handlers;

public class NostrHandler : WebSocketHandler
{
    public NostrHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
    {
    }

    public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
    {
        throw new NotImplementedException();
    }
}