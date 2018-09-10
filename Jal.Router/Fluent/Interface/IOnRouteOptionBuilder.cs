using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder OnErrorSendFailedMessageTo(string endpointname);
        IOnRouteOptionBuilder UseMiddleware(Action<IInboundMiddlewareBuilder> action);
        IOnRouteOptionBuilder OnEntry(Action<IOnRouteEntryBuilder> action);
        IOnRouteOptionBuilder ForwardMessageTo(string endpointname);
        IOnRouteOptionBuilder AsClaimCheck();
    }
}