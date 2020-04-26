using Jal.ChainOfResponsability;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class RouteMiddlewareBuilder : IRouteMiddlewareBuilder
    {
        private readonly Route _route;
        public RouteMiddlewareBuilder(Route route)
        {
            _route = route;
        }

        public void Add<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>
        {
            _route.Middlewares.Add(typeof(TMiddleware));
        }
    }
}