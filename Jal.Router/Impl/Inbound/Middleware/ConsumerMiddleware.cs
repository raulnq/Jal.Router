using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        public ConsumerMiddleware(IComponentFactoryGateway factory, IConsumer consumer):base(factory, consumer)
        {
        }

        public Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            return Consume(context.Data);
        }
    }
}