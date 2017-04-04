using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterLogger : IRouterLogger
    {
        public static IRouterLogger Instance = new NullRouterLogger();

        public virtual void OnEntry(InboundMessageContext context)
        {

        }

        public virtual void OnSuccess<TContent>(InboundMessageContext context, TContent content)
        {

        }

        public virtual void OnExit(InboundMessageContext context, long duration)
        {

        }

        public virtual void OnException(InboundMessageContext context, Exception exception)
        {

        }
    }
}