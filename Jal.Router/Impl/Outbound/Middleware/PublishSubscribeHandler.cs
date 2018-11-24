using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{
    public class PublishSubscribeHandler : IMiddleware<MessageContext>
    {
        private readonly ISender _sender;

        public PublishSubscribeHandler(ISender sender)
        {
            _sender = sender;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            _sender.Send(context.Data.Channel, context.Data);
        }
    }
}