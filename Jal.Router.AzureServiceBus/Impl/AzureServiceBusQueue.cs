using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbound;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override void Send<TContent>(OutboundMessageContext<TContent> context, IMessageAdapter adapter)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = adapter.Write<TContent, BrokeredMessage>(context);

            queueclient.Send(message);

            queueclient.Close();
        }

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }
    }
}