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
    public class MessageHandler : AbstractInboundMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly IMessageRouter _router;

        public MessageHandler(IMessageRouter router, IComponentFactoryGateway factory, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route);

            await CreateMessageEntityAndSave(context.Data).ConfigureAwait(false);

            await _router.Route(context.Data).ConfigureAwait(false);
        }
    }
}