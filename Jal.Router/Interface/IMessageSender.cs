namespace Jal.Router.Interface
{
    public interface IMessageSender<in TMessage> : IMessageSender
    {
        void Send(TMessage message);

        void Send(TMessage message, dynamic context);

    }

    public interface IMessageSender
    {
        
    }
}