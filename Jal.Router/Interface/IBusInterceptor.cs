using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusInterceptor
    {
        void OnSendEntry(OutboundMessageContext context, Options options);

        void OnSendExit(OutboundMessageContext context, Options options, long duration);

        void OnSendSuccess(OutboundMessageContext context, Options options);

        void OnReplyError(OutboundMessageContext context, Options options, Exception ex);

        void OnReplyEntry(OutboundMessageContext context, Options options);

        void OnReplyExit(OutboundMessageContext context, Options options, long duration);

        void OnReplySuccess(OutboundMessageContext context, Options options);

        void OnSendError(OutboundMessageContext context, Options options,  Exception ex);
    }
}