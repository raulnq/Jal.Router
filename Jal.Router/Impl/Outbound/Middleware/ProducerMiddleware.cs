using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ProducerMiddleware : AbstractProducerMiddleware, IAsyncMiddleware<MessageContext>
    {
        private readonly IProducer _producer;

        public ProducerMiddleware(IProducer producer, IComponentFactoryFacade factory) : base(factory)
        {
            _producer = producer;
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            
            try
            {
                await _producer.Produce(context.Data).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(context.Data).ConfigureAwait(false);
            }
            
        }
    }
}