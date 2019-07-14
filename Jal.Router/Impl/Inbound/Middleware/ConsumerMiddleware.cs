using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class ConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _consumer;

        public ConsumerMiddleware(IConsumer consumer, IComponentFactoryGateway factory, IConfiguration configuration):base(configuration, factory)
        {
            _consumer = consumer;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            messagecontext.TrackingContext.Add();

            try
            {
                await _consumer.Consume(messagecontext).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
            }
        }
    }
}