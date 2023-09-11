using Nostrfi.Relay.Handlers;

namespace Nostrfi.Relay;

public class WebSocketMiddleWare : IMiddleware
{
    private readonly ILogger<WebSocketMiddleWare> _logger;
    private readonly WebSocketHandler _handler;

    public WebSocketMiddleWare(ILogger<WebSocketMiddleWare> logger, WebSocketHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.WebSockets.IsWebSocketRequest) await next.Invoke(context);

        var socket = await context.WebSockets.AcceptWebSocketAsync();
        _logger.LogInformation("New websocket connection: {ConnectionId}", context.Connection.Id);
        await _handler.OnConnected(socket);
        return;

    }
}