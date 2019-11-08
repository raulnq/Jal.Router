using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ProducerMiddleware : AbstractProducerMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly IProducer _producer;

        public ProducerMiddleware(IProducer producer, IComponentFactoryGateway factory, IConfiguration configuration) : base(configuration, factory)
        {
            _producer = producer;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            
            try
            {
                await _producer.Send(context.Data).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(context.Data).ConfigureAwait(false);
            }
            
        }
    }
}