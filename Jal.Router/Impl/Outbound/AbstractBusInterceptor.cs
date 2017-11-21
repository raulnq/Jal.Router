using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public virtual void OnEntry(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnExit(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnSuccess(OutboundMessageContext context, Options options)
        {
        }

        public virtual void OnError(OutboundMessageContext context, Options options, Exception ex)
        {
        }
    }
}