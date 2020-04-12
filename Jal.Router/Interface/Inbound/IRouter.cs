using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouter
    {
        Task Route<TMiddleware>(MessageContext context) where TMiddleware : IAsyncMiddleware<MessageContext>;
    }
}