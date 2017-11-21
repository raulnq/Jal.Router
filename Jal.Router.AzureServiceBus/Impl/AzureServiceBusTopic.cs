using System;
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
        public override void Send<TContent>(OutboundMessageContext<TContent> context, IMessageBodyAdapter messageadapter,
            IMessageMetadataAdapter messagecontextadapter)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = messageadapter.Write<TContent, BrokeredMessage>(context.Content);

            message = messagecontextadapter.Create(context, message);

            topicClient.Send(message);

            topicClient.Close();
        }

        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }
    }
}