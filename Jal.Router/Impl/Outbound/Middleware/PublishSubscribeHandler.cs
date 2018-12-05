using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{
    public class PublishSubscribeHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly ISender _sender;

        public PublishSubscribeHandler(ISender sender, IComponentFactory factory, IConfiguration configuration) : base(configuration, factory)
        {
            _sender = sender;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            CreateOutboundMessageEntity(context.Data);

            _sender.Send(context.Data.Channel, context.Data);
        }
    }
}