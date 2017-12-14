using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IMiddleware
    {
        void Execute<TContent>(MessageContext<TContent> context, Action next, MiddlewareParameter parameter);
    }
}