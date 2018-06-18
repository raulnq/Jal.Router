using System;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface.Outbound
{
    public interface IMiddleware
    {
        void Execute(MessageContext context, Action next, Action current, MiddlewareParameter parameter);
    }
}