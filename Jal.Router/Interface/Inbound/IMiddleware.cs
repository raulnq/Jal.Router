using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IMiddleware
    {
        void Execute(MessageContext context, Action<MessageContext, MiddlewareContext> next, MiddlewareContext middlewarecontext);
    }
}