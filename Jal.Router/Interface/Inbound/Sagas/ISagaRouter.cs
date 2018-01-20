using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound.Sagas
{
    public interface ISagaRouter
    {
        void Start(object message, Saga saga, Route route);

        void Continue(object message, Saga saga, Route route);

        void End(object message, Saga saga, Route route);
    }
}