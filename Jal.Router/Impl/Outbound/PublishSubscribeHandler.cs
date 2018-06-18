using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class PublishSubscribeHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IChannelPathBuilder _builder;

        public PublishSubscribeHandler(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
        {
            _factory = factory;
            _configuration = configuration;
            _builder = builder;
        }

        public void Execute(MessageContext context, Action next, Action current, MiddlewareParameter parameter)
        {
            if (!string.IsNullOrWhiteSpace(parameter.Channel.ToConnectionString) && !string.IsNullOrWhiteSpace(parameter.Channel.ToPath))
            {
                var channelpath = _builder.BuildFromEndpoint(context.EndPoint.Name, parameter.Channel);

                var channel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

                channel.Send(parameter.Channel, context, channelpath);
            }
        }
    }
}