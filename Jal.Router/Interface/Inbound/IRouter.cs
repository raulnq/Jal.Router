using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouter
    {
        void Route<TMiddleware>(MessageContext context) where TMiddleware : IMiddleware<MessageContext>;
    }
}