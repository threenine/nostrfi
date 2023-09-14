using System.Net.WebSockets;
using System.Text;

namespace Nostrfi.Nostrize.Handlers;

public abstract class WebSocketHandler 
{
   protected ConnectionManager WebSocketConnectionManager { get; set; }

   protected WebSocketHandler(ConnectionManager webSocketConnectionManager)
   {
       WebSocketConnectionManager = webSocketConnectionManager;
   }
   
   public virtual Task OnConnected(WebSocket socket)
   {
       WebSocketConnectionManager.AddSocket(socket);
         return Task.CompletedTask;
   }
   
   public virtual async Task OnDisconnected(WebSocket socket)
   {
       await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
   }
   
   public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
}
