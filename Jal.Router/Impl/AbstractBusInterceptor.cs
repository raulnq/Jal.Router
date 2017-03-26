using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public static IBusInterceptor Instance = new NullBusInterceptor();
        public virtual void OnEntry(OutboundMessageContext context, string method)
        {
        }

        public virtual void OnExit(OutboundMessageContext context, string method, long duration)
        {
        }

        public virtual void OnSuccess(OutboundMessageContext context, string method)
        {
        }

        public virtual void OnError(OutboundMessageContext context, string method, Exception ex)
        {
        }
    }
}