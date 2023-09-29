using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nostrfi.Nostrize.Handlers;

namespace Nostrfi.Nostrize;

public class WebSocketMiddleware : IMiddleware
{
    private readonly ILogger<WebSocketMiddleware> _logger;
    private readonly NostrHandler _handler;

    public WebSocketMiddleware(ILogger<WebSocketMiddleware> logger, NostrHandler handler)
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


        async void HandleMessage(WebSocketReceiveResult result, string buffer)
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
                    throw new ArgumentOutOfRangeException
                        { HelpLink = null, HResult = 0, Source = nameof(WebSocketMiddleware), };
            }
        }
    }

    private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
    {
       
        while (socket.State == WebSocketState.Open)
        {
            string? message = null;
            try
            {
                var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
                WebSocketReceiveResult? result;
                await using (var ms = new MemoryStream())
                {
                    do
                    {
                         result = await socket.ReceiveAsync(buffer:buffer, cancellationToken: CancellationToken.None);
                         ms.Write(buffer.Array, buffer.Offset, result.Count);
                         
                    }while (!result.EndOfMessage);
                    
                    ms.Seek(0, SeekOrigin.Begin);
                    using(var rdr = new StreamReader(ms, Encoding.UTF8))
                    {
                        message = await rdr.ReadToEndAsync();
                        Console.WriteLine(message);
                    }
                }
                
                handleMessage(result, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        await _handler.OnDisconnected(socket);
    }
}