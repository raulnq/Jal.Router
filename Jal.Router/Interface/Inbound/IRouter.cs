using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouter
    {
        //void Route<TContent, TMessage>(TMessage message);

        void Route<TContent, TMessage>(TMessage message, Route route);

        //void RouteToSaga<TContent, TMessage>(TMessage message, string saganame);

        void Route<TContent, TMessage>(TMessage message, Saga saga, Route route, bool startingroute);
    }
}