using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterInterceptor : IRouterInterceptor
    {
        public static IRouterInterceptor Instance = new NullRouterInterceptor();


        public virtual void OnEntry(InboundMessageContext context)
        {

        }

        public virtual void OnSuccess<TContent>(InboundMessageContext context, TContent content)
        {

        }

        public virtual void OnExit(InboundMessageContext context)
        {

        }

        public virtual void OnException(InboundMessageContext context, Exception exception)
        {

        }
    }
}
