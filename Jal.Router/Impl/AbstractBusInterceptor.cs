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

        public virtual void OnSendExit(OutboundMessageContext context, Options options, long duration)
        {
        }

        public virtual void OnSendSuccess(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnReplyError(OutboundMessageContext context, Options options, Exception ex)
        {

        }

        public virtual void OnReplyEntry(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnReplyExit(OutboundMessageContext context, Options options, long duration)
        {

        }

        public virtual void OnReplySuccess(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {
        }
    }
}