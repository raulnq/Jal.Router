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

        public PublishSubscribeHandler(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var channel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

            channel.Send(context);
        }
    }
}