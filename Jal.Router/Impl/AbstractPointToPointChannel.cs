using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbound;

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

        public abstract void Send<TContent>(OutboundMessageContext<TContent> context, IMessageAdapter adapter);

        public void Send<TContent>(OutboundMessageContext<TContent> context)
        {
            var adapter = Factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            Send(context, adapter);
        }
    }
}