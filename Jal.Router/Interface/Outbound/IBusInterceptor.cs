using System;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface.Outbound
{
    public interface IBusInterceptor
    {
        void OnEntry(OutboundMessageContext context, Options options);

        void OnExit(OutboundMessageContext context, Options options);

        void OnSuccess(OutboundMessageContext context, Options options);

        void OnError(OutboundMessageContext context, Options options, Exception ex);

    }
}