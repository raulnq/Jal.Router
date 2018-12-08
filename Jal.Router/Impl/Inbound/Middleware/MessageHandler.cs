using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class MessageHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly IMessageRouter _router;

        public MessageHandler(IMessageRouter router, IComponentFactory factory, IConfiguration configuration):base(configuration, factory)
        {
            _router = router;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route);

            CreateMessageEntity(context.Data);

            _router.Route(context.Data);
        }
    }
}