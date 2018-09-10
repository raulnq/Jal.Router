using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class PointToPointHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public PointToPointHandler(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext)
        {
            if (middlewarecontext.Channel.IsValidEndpoint())
            {
                var channel = _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);

                channel.Send(middlewarecontext.Channel, context, middlewarecontext.Channel.GetPath(context.EndPoint.Name));
            }

            return null;
        }
    }
}