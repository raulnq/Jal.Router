using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IInboundMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IMiddleware<MessageContext>;
    }
}