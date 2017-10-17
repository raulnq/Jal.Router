using System;
using Jal.Router.Model;
using Jal.Router.Model.Outbount;

namespace Jal.Router.Interface.Outbound
{
    public interface IBusLogger
    {
        void OnSendEntry(OutboundMessageContext context, Options options);

        void OnSendExit(OutboundMessageContext context, Options options, long duration);

        void OnSendSuccess(OutboundMessageContext context, Options options);

        void OnSendError(OutboundMessageContext context, Options options, Exception ex);

        void OnPublishEntry(OutboundMessageContext context, Options options);

        void OnPublishExit(OutboundMessageContext context, Options options, long duration);

        void OnPublishSuccess(OutboundMessageContext context, Options options);

        void OnPublishError(OutboundMessageContext context, Options options, Exception ex);
    }
}