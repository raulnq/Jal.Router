namespace Jal.Router.Interface
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler<TMessage>[] Create<TMessage>(TMessage message, string route);
    }
}