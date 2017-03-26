using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusInterceptor
    {
        void OnEntry(OutboundMessageContext context, string method);

        void OnExit(OutboundMessageContext context, string method, long duration);

        void OnSuccess(OutboundMessageContext context, string method);

        void OnError(OutboundMessageContext context, string method, Exception ex);
    }
}