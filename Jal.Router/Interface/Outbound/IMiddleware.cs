using System;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface.Outbound
{
    public interface IMiddleware
    {
        void Execute<TContent>(OutboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter);
    }
}