using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{
    public class PointToPointHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly ISender _sender;

        public PointToPointHandler(ISender sender, IComponentFactory factory, IConfiguration configuration):base(configuration, factory)
        {
            _sender = sender;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            CreateMessageEntity(context.Data, MessageEntityType.Outbound);

            _sender.Send(context.Data.Channel, context.Data);
        }
    }
}