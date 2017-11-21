using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{
    public abstract class AbstractPublishSubscribeChannel : IPublishSubscribeChannel
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        protected AbstractPublishSubscribeChannel(IComponentFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public abstract void Send<TContent>(OutboundMessageContext<TContent> context, IMessageBodyAdapter messageadapter, IMessageMetadataAdapter messagecontextadapter);

        public void Send<TContent>(OutboundMessageContext<TContent> context)
        {
            var adapter = _factory.Create<IMessageBodyAdapter>(_configuration.MessageBodyAdapterType);

            var contextadapter = _factory.Create<IMessageMetadataAdapter>(_configuration.MessageMetadataAdapterType);

            Send(context, adapter, contextadapter);
        }
    }
}