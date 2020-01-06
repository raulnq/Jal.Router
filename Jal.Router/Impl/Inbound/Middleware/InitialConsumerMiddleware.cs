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
        private const string DefaultStatus = "STARTED";

        public InitialConsumerMiddleware(IComponentFactoryGateway factory, IConsumer consumer):base(factory, consumer)
        {
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            var storage = Factory.CreateEntityStorage();

            var sagadata = messagecontext.SagaContext.Create(DefaultStatus);

            var id = await storage.Create(sagadata).ConfigureAwait(false);

            sagadata.SetId(id);

            messagecontext.SagaContext.Load(sagadata);

            messagecontext.SagaContext.SetId(id);

            await Consume(messagecontext);

            await storage.Update(sagadata).ConfigureAwait(false);
        }
    }
}