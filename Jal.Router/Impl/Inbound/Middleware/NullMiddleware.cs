using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMiddleware : IMiddlewareAsync<MessageContext>
    {
        public Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            return Task.CompletedTask;
        }
    }
}