using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class FinalConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _consumer;

        private const string DefaultStatus = "ENDED";

        public FinalConsumerMiddleware(IComponentFactoryGateway factory, IConsumer consumer, IConfiguration configuration) : base(configuration, factory)
        {
            _consumer = consumer;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            messagecontext.SagaContext.Load(await storage.Get(messagecontext.SagaContext.Id).ConfigureAwait(false));

            if (messagecontext.SagaContext.IsLoaded())
            {
                messagecontext.SagaContext.Data.SetStatus(DefaultStatus);

                context.Data.TrackingContext.AddEntry();

                if (context.Data.SagaContext.Data.IsValid())
                {
                    try
                    {
                        await _consumer.Consume(context.Data).ConfigureAwait(false);

                        messagecontext.SagaContext.Data.End(messagecontext.DateTimeUtc);
                    }
                    finally
                    {
                        await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
                    }

                    await storage.Update(messagecontext.SagaContext.Data).ConfigureAwait(false);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid saga record data {messagecontext.Saga.DataType.FullName}, {messagecontext.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No saga record type {messagecontext.Saga.DataType.FullName}, {messagecontext.Name}");
            }
        }
    }
}