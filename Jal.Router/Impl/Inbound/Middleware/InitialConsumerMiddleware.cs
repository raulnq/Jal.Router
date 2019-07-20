using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class InitialConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _consumer;

        private const string DefaultStatus = "STARTED";

        public InitialConsumerMiddleware(IComponentFactoryGateway factory, IConsumer consumer, IConfiguration configuration):base(configuration, factory)
        {
            _consumer = consumer;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            var sagadata = messagecontext.SagaContext.CreateSagaData(DefaultStatus);

            await storage.CreateSagaData(messagecontext, sagadata).ConfigureAwait(false);

            messagecontext.SagaContext.UpdateSagaData(sagadata);

            messagecontext.TrackingContext.Add();

            try
            {
                await _consumer.Consume(messagecontext).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
            }

            await storage.UpdateSagaData(messagecontext, sagadata).ConfigureAwait(false);
        }
    }
}