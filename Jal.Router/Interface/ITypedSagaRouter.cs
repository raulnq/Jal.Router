using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ITypedSagaRouter
    {
        void Continue<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new();

        void Start<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new();
    }
}