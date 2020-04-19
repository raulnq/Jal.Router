using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class MiddleConsumerMiddleware : AbstractConsumerMiddleware, IAsyncMiddleware<MessageContext>
    {
        private const string DefaultStatus = "IN PROCESS";

        public MiddleConsumerMiddleware(IComponentFactoryFacade factory, IConsumer consumer) : base(factory, consumer)
        {
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            messagecontext.SagaContext.Load(await storage.Get(messagecontext.SagaContext.Id).ConfigureAwait(false));

            if (messagecontext.SagaContext.IsLoaded())
            {
                if (messagecontext.SagaContext.Data.IsValid())
                {
                    messagecontext.SagaContext.Data.SetStatus(DefaultStatus);

                    await Consume(messagecontext).ConfigureAwait(false);

                    messagecontext.SagaContext.Data.Update(messagecontext.DateTimeUtc);

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