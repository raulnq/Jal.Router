using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRouterBuilder
    {
        IRouterEndBuilder UseRouteConfigurationSource(IRouterConfigurationSource[] routerConfigurationSources);
    }
}