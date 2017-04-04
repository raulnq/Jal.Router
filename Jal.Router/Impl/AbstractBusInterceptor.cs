using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public static IBusInterceptor Instance = new NullBusInterceptor();
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