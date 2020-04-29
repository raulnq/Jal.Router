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

            await messagecontext.SagaContext.CreateAndInsertDataIntoStorage(DefaultStatus).ConfigureAwait(false);

            await Consume(messagecontext).ConfigureAwait(false);

            await messagecontext.SagaContext.UpdateIntoStorage().ConfigureAwait(false);
        }
    }
}