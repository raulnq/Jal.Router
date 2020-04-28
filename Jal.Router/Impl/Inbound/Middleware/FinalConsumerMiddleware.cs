using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class FinalConsumerMiddleware : AbstractConsumerMiddleware, IAsyncMiddleware<MessageContext>
    {
        private const string DefaultStatus = "ENDED";

        public FinalConsumerMiddleware(IComponentFactoryFacade factory, IConsumer consumer) : base(factory, consumer)
        {
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            await messagecontext.SagaContext.LoadDataFromStorage().ConfigureAwait(false);

            if (!messagecontext.SagaContext.IsLoaded())
            {
                throw new ApplicationException($"No saga record type {messagecontext.Saga.DataType.FullName}, {messagecontext.Name}");
            }

            if (!messagecontext.SagaContext.Data.IsValid())
            {
                throw new ApplicationException($"Empty/Invalid saga record data {messagecontext.Saga.DataType.FullName}, {messagecontext.Name}");
            }

            messagecontext.SagaContext.Data.SetStatus(DefaultStatus);

            await Consume(messagecontext).ConfigureAwait(false);

            messagecontext.SagaContext.Data.End(messagecontext.DateTimeUtc);

            await messagecontext.SagaContext.UpdateIntoStorage().ConfigureAwait(false);
        }
    }
}