using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nostrfi.Nostrize.Handlers;

namespace Nostrfi.Nostrize;

public class WebSocketMiddleware :  IMiddleware
{
private readonly ILogger<WebSocketMiddleware> _logger;
private readonly WebSocketHandler _handler;

public WebSocketMiddleware(ILogger<WebSocketMiddleware> logger, WebSocketHandler handler)
{
    _logger = logger;
    _handler = handler;
}
public async Task InvokeAsync(HttpContext context, RequestDelegate next)
{
    if (!context.WebSockets.IsWebSocketRequest) await next.Invoke(context);

    var socket = await context.WebSockets.AcceptWebSocketAsync();
    _logger.LogInformation("WebSocket connection request: {ContextId}", context.Connection.Id);
    await _handler.OnConnected(socket);

    await Receive(socket, HandleMessage);
    return;


    async void HandleMessage(WebSocketReceiveResult result, byte[] buffer)
    {
        switch (result.MessageType)
        {
            case WebSocketMessageType.Text:
                await _handler.ReceiveAsync(socket, result, buffer);
                break;
            case WebSocketMessageType.Close:
                await _handler.OnDisconnected(socket);
                break;
            case WebSocketMessageType.Binary:
                break;
            default:
                throw new ArgumentOutOfRangeException { HelpLink = null, HResult = 0, Source = nameof(WebSocketMiddleware), };
        }
    }
}
private static async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
{
    var buffer = new byte[1024 * 4];

    while(socket.State == WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
            cancellationToken: CancellationToken.None);

        handleMessage(result, buffer);
    }
}
}