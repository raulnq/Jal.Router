namespace Jal.Router.Interface
{
    public interface IMessageHandler<in TMessage> : IMessageHandler
    {
        void Handle(TMessage message);

        void Handle(TMessage message, dynamic context);

    }

    public interface IMessageHandler
    {
        
    }
}