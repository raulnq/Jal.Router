using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IMiddlewareBuilder
    {
        void Add<TMiddleware>() where TMiddleware : IMiddleware;
    }
}