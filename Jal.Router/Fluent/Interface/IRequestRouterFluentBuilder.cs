using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRequestRouterFluentBuilder : IRequestRouterEndFluentBuilder
    {
        IRequestRouterFluentBuilder UseInterceptor(IRequestRouterInterceptor requestRouterInterceptor);
    }
}