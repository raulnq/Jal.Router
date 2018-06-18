using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IInboundMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IMiddleware;
    }
}