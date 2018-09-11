using System;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface.Outbound
{
    public interface IMiddleware
    {
        object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext);
    }
}