using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbount;

namespace Jal.Router.Impl.Outbound
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public virtual void OnSendEntry(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnSendExit(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnSendSuccess(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
        }

        public virtual void OnPublishError(OutboundMessageContext context, Options options, Exception ex)
        {

        }

        public virtual void OnPublishEntry(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnPublishExit(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnPublishSuccess(OutboundMessageContext context, Options options)
        {

        }
    }
}