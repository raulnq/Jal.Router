using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractBusInterceptor : IBusInterceptor
    {
        public virtual void OnEntry(MessageContext context)
        {
        }

        public virtual void OnExit(MessageContext context)
        {
        }

        public virtual void OnSuccess(MessageContext context)
        {
        }

        public virtual void OnError(MessageContext context, Exception ex)
        {
        }
    }
}