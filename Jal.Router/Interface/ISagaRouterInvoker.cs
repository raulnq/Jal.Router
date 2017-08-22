using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ISagaRouterInvoker
    {
        void Continue<TContent>(Saga saga, InboundMessageContext<TContent> context, Route route);

        void Start<TContent>(Saga saga, InboundMessageContext<TContent> context, Route route);
    }
}