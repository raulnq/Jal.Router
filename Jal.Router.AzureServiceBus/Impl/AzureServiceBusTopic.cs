using System.Globalization;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : IPublisher
    {
        private readonly IMessageAdapter<BrokeredMessage> _adapter;

        private readonly IValueSettingFinderFactory _valueSettingFinderFactory;

        public AzureServiceBusTopic(IMessageAdapter<BrokeredMessage> adapter, IValueSettingFinderFactory valueSettingFinderFactory)
        {
            _adapter = adapter;
            _valueSettingFinderFactory = valueSettingFinderFactory;
        }

        public void Publish<TContent>(OutboundMessageContext<TContent> context)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = _adapter.Write(context);

            topicClient.Send(message);

            topicClient.Close();
        }
    }
}