using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IInterceptorRouterBuilder : IRouterEndBuilder
    {
        IRouterBuilder UseInterceptor(IRouterInterceptor routerInterceptor);
    }
}