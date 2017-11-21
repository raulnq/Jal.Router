using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbound;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : AbstractPublishSubscribeChannel
    {
        public override void Send<TContent>(OutboundMessageContext<TContent> context, IMessageAdapter adapter)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = adapter.Write<TContent, BrokeredMessage>(context);

            topicClient.Send(message);

            topicClient.Close();
        }

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }
    }
}