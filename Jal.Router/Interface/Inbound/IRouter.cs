namespace Jal.Router.Interface.Inbound
{
    public interface IRouter
    {
        void Route<TContent, TMessage>(TMessage message, string routename = "");

        void RouteToSaga<TContent, TMessage>(TMessage message, string saganame, string routename = "");
    }
}