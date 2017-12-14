using System;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public virtual void OnEntry(MessageContext context, Options options)
        {
        }

        public virtual void OnExit(MessageContext context, Options options)
        {
        }

        public virtual void OnSuccess(MessageContext context, Options options)
        {
        }

        public virtual void OnError(MessageContext context, Options options, Exception ex)
        {
        }
    }
}