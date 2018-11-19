using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class RequestReplyHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly ISender _sender;

        public RequestReplyHandler(IComponentFactory factory, IConfiguration configuration, ISender sender)
        {
            _factory = factory;
            _configuration = configuration;
            _sender = sender;
        }

        public object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext)
        {
            //if (middlewarecontext.Channel.IsValidReplyEndpoint() && !string.IsNullOrWhiteSpace(context.Identity.ReplyToRequestId))
            //{
            //    var channel = _factory.Create<IRequestReplyChannel>(_configuration.RequestReplyChannelType);

            //    var result = channel.Reply(middlewarecontext.Channel, context, middlewarecontext.Channel.GetPath(context.EndPoint.Name));

            //    return result;
            //}

            return _sender.Send(middlewarecontext.Channel, context);
        }
    }
}