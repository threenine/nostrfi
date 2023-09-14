using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Nostrfi.Nostrize;

public class ConnectionManager
{
    private const string Dash = "-";
    private ConcurrentDictionary<string, WebSocket> _sockets = new();
    
    public WebSocket GetSocketById(string id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }
    
    public ConcurrentDictionary<string, WebSocket> GetAll()
    {
        return _sockets;
    }
    
    public string GetId(WebSocket socket)
    {
        return _sockets.FirstOrDefault(p => p.Value == socket).Key;
    }
    
    public void AddSocket(WebSocket socket)
    {
        _sockets.TryAdd(ConnectionId, socket);
    }
    
    public async Task RemoveSocket(string id)
    {
        _sockets.TryRemove(id, out var socket);
        if (socket == null) return;
        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, Feedback.ClosedByConnectionManager, CancellationToken.None);
    }
    
    private static string ConnectionId => Guid.NewGuid().ToString().Replace(Dash, string.Empty);
    
}