using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public static IBusInterceptor Instance = new NullBusInterceptor();
        public virtual void OnEntry(OutboundMessageContext context, Options options, string method)
        {
        }

        public virtual void OnExit(OutboundMessageContext context, Options options, long duration, string method)
        {
        }

        public virtual void OnSuccess(OutboundMessageContext context, Options options, string method)
        {
        }

        public virtual void OnError(OutboundMessageContext context, Options options, Exception ex, string method)
        {
        }
    }
}