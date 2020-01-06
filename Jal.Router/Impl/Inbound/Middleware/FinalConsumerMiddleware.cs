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
        private const string DefaultStatus = "ENDED";

        public FinalConsumerMiddleware(IComponentFactoryGateway factory, IConsumer consumer) : base(factory, consumer)
        {
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            messagecontext.SagaContext.Load(await storage.Get(messagecontext.SagaContext.Id).ConfigureAwait(false));

            if (messagecontext.SagaContext.IsLoaded())
            {
                if (messagecontext.SagaContext.Data.IsValid())
                {
                    messagecontext.SagaContext.Data.SetStatus(DefaultStatus);

                    await Consume(messagecontext);

                    messagecontext.SagaContext.Data.End(messagecontext.DateTimeUtc);

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