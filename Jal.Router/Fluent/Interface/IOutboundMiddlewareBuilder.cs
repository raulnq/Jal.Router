using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOutboundMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>;
    }
}