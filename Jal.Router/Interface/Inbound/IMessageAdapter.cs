namespace Jal.Router.Interface.Inbound
{
    public interface IMessageBodyAdapter
    {
        TContent Read<TContent, TMessage>(TMessage message);

        TMessage Write<TContent, TMessage>(TContent content);        
    }
}