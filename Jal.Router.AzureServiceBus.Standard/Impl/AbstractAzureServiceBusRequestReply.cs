using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model.Outbound;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureServiceBusRequestReply : AbstractChannel
    {
        protected AbstractAzureServiceBusRequestReply(IComponentFactoryGateway factory,  ILogger logger)
            : base(factory, logger)
        {

        }

        protected SenderMetadata _metadata;

        private QueueClient _client;

        public void Open(SenderMetadata metadata)
        {
            _client = new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);

            _metadata = metadata;
        }

        public async Task<string> Send(object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                await _client.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public Task Close()
        {
            return _client.CloseAsync();
        }

    }
}