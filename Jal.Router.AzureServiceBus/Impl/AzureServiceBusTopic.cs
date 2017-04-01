using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : IPublisher
    {
        private readonly IBrokeredMessageContentAdapter _contentAdapter;

        private readonly IBrokeredMessageFromAdapter _fromAdapter;

        private readonly IBrokeredMessageIdAdapter _idAdapter;

        public AzureServiceBusTopic(IBrokeredMessageContentAdapter contentAdapter, IBrokeredMessageFromAdapter fromAdapter, IBrokeredMessageIdAdapter idAdapter)
        {
            _contentAdapter = contentAdapter;
            _fromAdapter = fromAdapter;
            _idAdapter = idAdapter;
        }

        public void Publish<TContent>(OutboundMessageContext<TContent> context)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = _contentAdapter.Writer(context.Content);

            _idAdapter.Write(context.Id, message);

            _fromAdapter.Write(context.From, message);

            _fromAdapter.WriteOrigin(context.Origin, message);

            foreach (var header in context.Headers)
            {
                message.Properties.Add(header.Key, header.Value);
            }

            topicClient.Send(message);

            topicClient.Close();
        }
    }
}