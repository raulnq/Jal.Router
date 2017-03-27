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

        private readonly IBrokeredMessageReplyToAdapter _replyToAdapter;

        private readonly IBrokeredMessageIdAdapter _idAdapter;

        public AzureServiceBusQueue(IBrokeredMessageContentAdapter contentAdapter, IBrokeredMessageFromAdapter fromAdapter, IBrokeredMessageReplyToAdapter replyToAdapter, IBrokeredMessageIdAdapter idAdapter)
        {
            _contentAdapter = contentAdapter;
            _fromAdapter = fromAdapter;
            _replyToAdapter = replyToAdapter;
            _idAdapter = idAdapter;
        }

        public void Enqueue<TContent>(OutboundMessageContext<TContent> context)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = _contentAdapter.Writer(context.Content);

            _replyToAdapter.WritePath(context.ReplyToPath, message);

            _replyToAdapter.WriteConnectionString(context.ReplyToConnectionString, message);

            _replyToAdapter.Write(context.ReplyTo, message);

            if (!string.IsNullOrWhiteSpace(context.ReplyToConnectionString) && !string.IsNullOrWhiteSpace(context.ReplyToPath)) //TODO delete
            {
                message.ReplyTo = $"{context.ReplyToConnectionString};queue={context.ReplyToPath}";
            }

            message.CorrelationId = context.Id;//TODO delete

            _idAdapter.Write(context.Id, message);

            _fromAdapter.Write(context.From, message);

            queueclient.Send(message);

            queueclient.Close();
        }
    }
}