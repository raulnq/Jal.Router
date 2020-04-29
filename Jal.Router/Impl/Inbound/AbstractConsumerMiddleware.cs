using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractConsumerMiddleware : AbstractMiddleware
    {
        private readonly IConsumer _consumer;

        protected AbstractConsumerMiddleware(IComponentFactoryFacade factory, IConsumer consumer) : base(factory)
        {
            _consumer = consumer;
        }

        protected async Task Consume(MessageContext messagecontext)
        {
            messagecontext.TrackingContext.AddEntry();

            try
            {
                await _consumer.Consume(messagecontext).ConfigureAwait(false);
            }
            finally
            {
                await CreateAndInsertMessageIntoStorage(messagecontext).ConfigureAwait(false);
            }
        }
    }
}