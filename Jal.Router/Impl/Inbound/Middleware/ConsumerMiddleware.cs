using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ConsumerMiddleware : AbstractConsumerMiddleware, IAsyncMiddleware<MessageContext>
    {
        public ConsumerMiddleware(IComponentFactoryFacade factory, IConsumer consumer):base(factory, consumer)
        {
        }

        public Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            return Consume(context.Data);
        }
    }
}