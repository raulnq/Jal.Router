using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbount;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override void Send<TContent>(OutboundMessageContext<TContent> context, IMessageBodyAdapter messageadapter, IMessageMetadataAdapter messagecontextadapter)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = messageadapter.Write<TContent,BrokeredMessage>(context.Content);

            message = messagecontextadapter.Create(context, message);

            queueclient.Send(message);

            queueclient.Close();
        }

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }
    }
}