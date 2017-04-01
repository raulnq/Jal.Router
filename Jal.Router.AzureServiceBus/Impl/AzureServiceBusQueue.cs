using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : IQueue
    {
        private readonly IBrokeredMessageContentAdapter _contentAdapter;

        private readonly IBrokeredMessageFromAdapter _fromAdapter;

        private readonly IBrokeredMessageIdAdapter _idAdapter;

        public AzureServiceBusQueue(IBrokeredMessageContentAdapter contentAdapter, IBrokeredMessageFromAdapter fromAdapter,IBrokeredMessageIdAdapter idAdapter)
        {
            _contentAdapter = contentAdapter;
            _fromAdapter = fromAdapter;
            _idAdapter = idAdapter;
        }

        public void Enqueue<TContent>(OutboundMessageContext<TContent> context)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = _contentAdapter.Writer(context.Content);

            _idAdapter.Write(context.Id, message);

            _fromAdapter.Write(context.From, message);

            _fromAdapter.WriteOrigin(context.Origin, message);

            foreach (var header in context.Headers)
            {
                message.Properties.Add(header.Key,header.Value);
            }

            queueclient.Send(message);

            queueclient.Close();
        }
    }
}