using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterLogger
    {
        void OnEntry(InboundMessageContext context);

        void OnSuccess<TContent>(InboundMessageContext context, TContent content);

        void OnExit(InboundMessageContext context, long duration);

        void OnException(InboundMessageContext context, Exception exception);
    }
}