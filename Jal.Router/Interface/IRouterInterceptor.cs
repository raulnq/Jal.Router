using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterInterceptor
    {
        void OnEntry(InboundMessageContext context);

        void OnSuccess<TContent>(InboundMessageContext context, TContent content);

        void OnExit(InboundMessageContext context);

        void OnException(InboundMessageContext context, Exception exception);
    }
}