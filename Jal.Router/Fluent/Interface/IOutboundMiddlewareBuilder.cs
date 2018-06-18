using Jal.Router.Interface.Outbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOutboundMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IMiddleware;
    }
}