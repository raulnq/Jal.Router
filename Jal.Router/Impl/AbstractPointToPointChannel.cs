using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbount;

namespace Jal.Router.Impl
{
    public abstract class AbstractPointToPointChannel : IPointToPointChannel
    {
        protected readonly IComponentFactory Factory;

        private readonly IConfiguration _configuration;

        protected AbstractPointToPointChannel(IComponentFactory factory, IConfiguration configuration)
        {
            Factory = factory;
            _configuration = configuration;
        }

        public abstract void Send<TContent>(OutboundMessageContext<TContent> context, IMessageBodyAdapter messageadapter, IMessageMetadataAdapter messagecontextadapter);

        public void Send<TContent>(OutboundMessageContext<TContent> context)
        {
            var adapter = Factory.Create<IMessageBodyAdapter>(_configuration.MessageBodyAdapterType);

            var contextadapter = Factory.Create<IMessageMetadataAdapter>(_configuration.MessageMetadataAdapterType);

            Send(context, adapter, contextadapter);
        }
    }
}