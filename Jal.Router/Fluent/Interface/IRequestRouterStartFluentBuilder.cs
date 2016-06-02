using Jal.Factory.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRequestRouterStartFluentBuilder
    {
        IRequestRouterFluentBuilder UseRouteProvider(IObjectFactory objectFactory);

        IRequestRouterFluentBuilder UseRouteProvider(IRouteProvider routeProvider);

        IRequestRouterEndFluentBuilder UseRequestRouter(IRequestRouter requestRouter);
    }
}