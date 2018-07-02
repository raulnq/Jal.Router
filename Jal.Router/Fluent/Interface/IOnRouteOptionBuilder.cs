using System;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder OnErrorSendFailedMessageTo(string endpointname);
        IOnRouteOptionBuilder UsingMiddleware(Action<IInboundMiddlewareBuilder> action);
        IOnRouteOptionBuilder ForwardMessageTo(string endpointname);
        IOnRouteOptionBuilder AsClaimCheck();
    }
}