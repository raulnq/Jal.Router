using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOutboundMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>;
    }
}