using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterInterceptor : IRouterInterceptor
    {
        public virtual void OnEntry(MessageContext context)
        {

        }

        public virtual void OnSuccess(MessageContext context)
        {

        }

        public virtual void OnExit(MessageContext context)
        {

        }

        public virtual void OnException(MessageContext context, Exception exception)
        {

        }
    }
}
