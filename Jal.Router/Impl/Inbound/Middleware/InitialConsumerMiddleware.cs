using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class InitialConsumerMiddleware : AbstractConsumerMiddleware, IAsyncMiddleware<MessageContext>
    {
        private const string DefaultStatus = "STARTED";

        public InitialConsumerMiddleware(IComponentFactoryFacade factory, IConsumer consumer):base(factory, consumer)
        {
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            var sagadata = messagecontext.SagaContext.Create(DefaultStatus);

            var id = await storage.Create(sagadata).ConfigureAwait(false);

            sagadata.SetId(id);

            messagecontext.SagaContext.Load(sagadata);

            messagecontext.SagaContext.SetId(id);

            await Consume(messagecontext).ConfigureAwait(false);

            await storage.Update(sagadata).ConfigureAwait(false);
        }
    }
}