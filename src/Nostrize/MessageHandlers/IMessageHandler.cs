namespace Nostrfi.Nostrize.MessageHandlers;

public interface IMessageHandler
{
    public Task Handle(string connectionId, string msg);
}