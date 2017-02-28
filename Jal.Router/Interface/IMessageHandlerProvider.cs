namespace Jal.Router.Interface
{
    public interface IMessageHandlerProvider
    {
        IMessageHandler<TMessage>[] Provide<TMessage>(TMessage message, string route);
    }
}