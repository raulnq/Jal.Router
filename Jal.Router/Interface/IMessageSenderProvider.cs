namespace Jal.Router.Interface
{
    public interface IMessageSenderProvider
    {
        IMessageSender<TMessage>[] Provide<TMessage>(TMessage message, string route);
    }
}