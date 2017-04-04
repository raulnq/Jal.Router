using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : IQueue
    {
        private readonly IMessageAdapter<BrokeredMessage> _adapter;
        public AzureServiceBusQueue(IMessageAdapter<BrokeredMessage> adapter)
        {
            _adapter = adapter;
        }

        public void Enqueue<TContent>(OutboundMessageContext<TContent> context)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = _adapter.Write(context);

            queueclient.Send(message);

            queueclient.Close();
        }
    }
}