using System;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IMiddleware
    {
        void Execute<TContent>(InboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter);
    }
}