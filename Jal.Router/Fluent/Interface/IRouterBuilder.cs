using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRouterBuilder
    {
        IInterceptorRouterBuilder UseRouteConfigurationSource(IRouterConfigurationSource[] routerConfigurationSources);
    }
}