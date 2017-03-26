using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusInterceptor
    {
        void OnEntry(OutboundMessageContext context, Options options, string method);

        void OnExit(OutboundMessageContext context, Options options, string method, long duration);

        void OnSuccess(OutboundMessageContext context, Options options, string method);

        void OnError(OutboundMessageContext context, Options options, string method, Exception ex);
    }
}