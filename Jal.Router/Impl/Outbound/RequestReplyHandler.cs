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

        public RequestReplyHandler(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, Action current, MiddlewareParameter parameter)
        {
            if (parameter.Channel.IsValidReplyEndpoint() && !string.IsNullOrWhiteSpace(context.Identity.ReplyToRequestId))
            {
                var channel = _factory.Create<IRequestReplyChannel>(_configuration.RequestReplyChannelType);

                var result = channel.Reply(parameter.Channel, context, parameter.Channel.GetPath(context.EndPoint.Name));

                parameter.Result = result;
            }
        }
    }
}