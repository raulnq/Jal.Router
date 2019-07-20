using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouter
    {
        Task Route<TMiddleware>(MessageContext context) where TMiddleware : IMiddlewareAsync<MessageContext>;
    }
}