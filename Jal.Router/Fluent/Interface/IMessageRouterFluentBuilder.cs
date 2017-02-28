using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IMessageRouterFluentBuilder : IMessageRouterEndFluentBuilder
    {
        IMessageRouterFluentBuilder UseInterceptor(IMessagetRouterInterceptor messagetRouterInterceptor);
    }
}