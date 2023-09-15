namespace Nostrfi.Nostrize.MessageHandlers;

/// <summary>
/// request events and subscribe to new updates
/// Upon receiving a REQ message, the relay SHOULD query its internal database and return events that match the filter, then store that filter and send again all future events it receives to that same websocket until the websocket is closed. The CLOSE event is received with the same &lt;subscription_id&gt; or a new REQ is sent using the same &lt;subscription_id&gt;, in which case relay MUST overwrite the previous subscription.
/// </summary>
public class RequestMessageHandler : IMessageHandler
{
    public Task Handle(string connectionId, string msg)
    {
        throw new NotImplementedException();
    }
}