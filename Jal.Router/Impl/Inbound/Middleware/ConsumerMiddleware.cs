using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class ConsumerMiddleware : AbstractConsumerMiddleware, IMiddlewareAsync<MessageContext>
    {
        private readonly IConsumer _router;

        public ConsumerMiddleware(IConsumer router, IComponentFactoryGateway factory, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var messagecontext = context.Data;

            messagecontext.TrackingContext.Add();

            try
            {
                await _router.Consume(messagecontext).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(messagecontext).ConfigureAwait(false);
            }
        }
    }
}