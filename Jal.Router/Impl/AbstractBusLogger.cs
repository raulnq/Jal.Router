using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractBusLogger : IBusLogger
    {
        public static IBusLogger Instance = new NullBusLogger();

        public virtual void OnSendEntry(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnSendExit(OutboundMessageContext context, Options options, long duration)
        {

        }

        public virtual void OnSendSuccess(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnSendError(OutboundMessageContext context, Options options, Exception ex)
        {

        }

        public virtual void OnPublishEntry(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnPublishExit(OutboundMessageContext context, Options options, long duration)
        {

        }

        public virtual void OnPublishSuccess(OutboundMessageContext context, Options options)
        {

        }

        public virtual void OnPublishError(OutboundMessageContext context, Options options, Exception ex)
        {

        }
    }
}