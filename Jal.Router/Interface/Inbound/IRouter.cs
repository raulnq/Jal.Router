using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouter
    {
        void Route(object message, Route route);

        void RouteToStartingSaga(object message, Saga saga, Route route);

        void RouteToContinueSaga(object message, Saga saga, Route route);
    }
}