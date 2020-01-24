using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureServiceBusRequestReply : AbstractChannel
    {
        protected AbstractAzureServiceBusRequestReply(IComponentFactoryGateway factory,  ILogger logger)
            : base(factory, logger)
        {

        }

        private QueueClient _client;

        public void Open(SenderContext sendercontext)
        {
            _client = new QueueClient(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

            if (sbmessage != null)
            {
                await _client.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return !_client.IsClosedOrClosing;
        }

        public Task Close(SenderContext sendercontext)
        {
            return _client.CloseAsync();
        }

    }
}