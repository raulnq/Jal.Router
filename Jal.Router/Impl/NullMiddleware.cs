using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMiddleware : IAsyncMiddleware<MessageContext>
    {
        public Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            return Task.CompletedTask;
        }
    }
}