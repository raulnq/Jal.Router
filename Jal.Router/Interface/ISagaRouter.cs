namespace Jal.Router.Interface
{
    public interface ISagaRouter<in TMessage>
    {
        void Route<TContent>(TMessage message, string saganame, string routename = "");
    }
}